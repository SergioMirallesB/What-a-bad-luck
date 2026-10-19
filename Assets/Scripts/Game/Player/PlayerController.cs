
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Inputs")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float acceleration = 50f;
    [SerializeField] private float deceleration = 60f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField, Range(0.1f, 1f)] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private int maxJumps = 2;

    [Header("Jump Assistance")]
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 18f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 0.1f;
    [SerializeField] private float dashGravityScale = 0f;

    private Rigidbody2D rb;

    private bool isGrounded;
    private bool isDashing;
    private bool canDash = true;

    private int jumpsUsed;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private float dashCounter;
    private float cooldownCounter;

    private float originalGravityScale;

    private Vector2 lastMoveDirection = Vector2.right;
    private Vector2 dashDirection = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravityScale = rb.gravityScale;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        dashAction.action.Enable();

        jumpAction.action.performed += OnJump;
        jumpAction.action.canceled += OnJumpCanceled;
        dashAction.action.performed += OnDash;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJump;
        jumpAction.action.canceled -= OnJumpCanceled;
        dashAction.action.performed -= OnDash;

        if (rb != null)
            rb.gravityScale = originalGravityScale;

        isDashing = false;

        moveAction.action.Disable();
        jumpAction.action.Disable();
        dashAction.action.Disable();
    }

    private void Update()
    {
        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.deltaTime;

        if (cooldownCounter > 0f)
            cooldownCounter -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Detectar suelo solo cuando no estamos subiendo.
        bool touchingGround = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        isGrounded =
            touchingGround &&
            rb.linearVelocity.y <= 0.05f &&
            !isDashing;

        if (isGrounded)
        {
            jumpsUsed = 0;
            canDash = true;
            coyoteCounter = coyoteTime;
        }
        else
        {
            coyoteCounter -= Time.fixedDeltaTime;
        }

        // Actualizar la dirección usada para el dash.
        Vector2 input =
            moveAction.action.ReadValue<Vector2>();

        if (input.sqrMagnitude > 0.01f)
            lastMoveDirection = input.normalized;

        // Durante el dash, el impulso tiene prioridad.
        if (isDashing)
        {
            dashCounter -= Time.fixedDeltaTime;

            rb.linearVelocity = dashDirection * dashSpeed;

            if (dashCounter <= 0f)
                EndDash();

            return;
        }

        // Movimiento normal.
        HandleMovement();

        // Salto normal, coyote time o doble salto.
        if (jumpBufferCounter > 0f)
        {
            bool canFirstJump =
                jumpsUsed == 0 &&
                (isGrounded || coyoteCounter > 0f);

            bool canExtraJump =
                jumpsUsed > 0 &&
                jumpsUsed < maxJumps;

            if (canFirstJump || canExtraJump)
            {
                PerformJump();
            }
        }
    }

    private void HandleMovement()
    {
        float inputX =
            moveAction.action.ReadValue<Vector2>().x;

        float targetSpeed = inputX * moveSpeed;

        float rate = Mathf.Abs(inputX) > 0.01f
            ? acceleration
            : deceleration;

        float newSpeed = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            rate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(
            newSpeed,
            rb.linearVelocity.y
        );
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        jumpBufferCounter = jumpBufferTime;
    }

    private void OnJumpCanceled(
        InputAction.CallbackContext context)
    {
        if (!isDashing && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(
            rb.linearVelocity.x,
            jumpForce
        );

        jumpsUsed++;
        jumpBufferCounter = 0f;
        coyoteCounter = 0f;
        isGrounded = false;
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (!canDash || isDashing || cooldownCounter > 0f)
            return;

        Vector2 input =
            moveAction.action.ReadValue<Vector2>();

        // Si no hay dirección, usar la última conocida.
        if (input.sqrMagnitude > 0.01f)
            dashDirection = input.normalized;
        else
            dashDirection = lastMoveDirection;

        StartDash();
    }

    private void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashCounter = dashDuration;

        rb.gravityScale = dashGravityScale;
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    private void EndDash()
    {
        isDashing = false;
        rb.gravityScale = originalGravityScale;
        cooldownCounter = dashCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundCheckRadius
        );
    }
}
