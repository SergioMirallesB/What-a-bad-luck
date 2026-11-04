
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 80f;
    [SerializeField] private float sprintModifier = 8f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float jumpCutMultiplier = 0.5f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float coyoteTime = 0.12f;
    [SerializeField] private float jumpBufferTime = 0.12f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference sprintAction;
    [Header("Particles")]
    [SerializeField] private TrailRenderer trail;

    [Header("Others")]
    private Vector2 lastCheckPoint = Vector2.zero;
    private Animator animator;
    private Rigidbody2D rb;
    private Collider2D collider;
    private HealthSystem healthManager;
    private float moveInput;
    private float coyoteCounter;
    private float jumpBufferCounter;
    private int jumpsUsed;
    private bool isGrounded;

    public static bool card = false;


    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip damageSound;

    private void Awake()
    {

        audioSource.PlayOneShot(gameMusic);

        audioSource.loop = true;

        audioSource.volume = 0.1f;

        rb = GetComponent<Rigidbody2D>();

        collider = GetComponent<Collider2D>();

        healthManager = GetComponent<HealthSystem>();

        animator = GetComponent<Animator>();
    }
    
    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
        sprintAction.action.Enable();

        jumpAction.action.performed += OnJump;
        jumpAction.action.canceled += OnJumpReleased;
        sprintAction.action.performed += OnSprint;
        sprintAction.action.canceled += OnSprintReleased;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJump;
        jumpAction.action.canceled -= OnJumpReleased;
        sprintAction.action.performed -= OnSprint;
        sprintAction.action.canceled -= OnSprintReleased;

        moveAction.action.Disable();
        jumpAction.action.Disable();
        sprintAction.action.Disable();
    }

    private void Update()
    {
        if (this.transform.position.y <= -10)
        {
            this.TeleportDamage(1);
        }
        
        moveInput = moveAction.action.ReadValue<float>();

        animator.SetBool("IsWalking", moveInput != 0.0f);
        
        if (moveInput < 0)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        } 
        else if (moveInput > 0)
        {

            this.transform.localScale = new Vector3(1, 1, 1);
        }

        CheckGround();
        UpdateTimers();
        HandleJump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckGround()
    {
        isGrounded = rb.linearVelocity.y <= 0.05f &&
            Physics2D.OverlapCircle(
                groundCheck.position,
                groundCheckRadius,
                groundLayer
            );

        if (isGrounded)
        {
            animator.SetBool("IsJumping", false);
            coyoteCounter = coyoteTime;
            jumpsUsed = 0;

        } else
        {

            animator.SetBool("IsJumping", true);

        }
    }

    private void UpdateTimers()
    {
        coyoteCounter -= Time.deltaTime;
        jumpBufferCounter -= Time.deltaTime;

        if (coyoteCounter <= 0) jumpsUsed++;
    }

    private void Move()
    {
        float targetSpeed = moveInput * moveSpeed;

        float rate = Mathf.Abs(targetSpeed) > 0.01f
            ? acceleration
            : deceleration;

        float newX = Mathf.MoveTowards(
            rb.linearVelocity.x,
            targetSpeed,
            rate * Time.fixedDeltaTime
        );

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);
        
    }

    private void OnJump(InputAction.CallbackContext context)
    {

        jumpBufferCounter = jumpBufferTime;

    }

    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        if (rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                rb.linearVelocity.y * jumpCutMultiplier
            );
        }
    }

    private void HandleJump()
    {
        if (jumpBufferCounter <= 0f)
            return;

        bool canCoyoteJump = coyoteCounter >= 0f && jumpsUsed == 0;
        bool canDoubleJump = jumpsUsed < maxJumps;

        if (canCoyoteJump || canDoubleJump)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                jumpForce
            );

            audioSource.PlayOneShot(jumpSound);

            jumpsUsed++;
            coyoteCounter = 0f;
            jumpBufferCounter = 0f;
        }
    }

    private void OnSprint(InputAction.CallbackContext contex)
    {
        this.moveSpeed += this.sprintModifier;
        
        this.trail.emitting = true;

        this.animator.speed = 2f;
    }

    private void OnSprintReleased(InputAction.CallbackContext contex)
    {
        this.moveSpeed -= this.sprintModifier;
        
        this.trail.emitting = false;

        this.animator.speed = 1f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Victory"))
        {
            GameManager.setLevel(GameManager.getLevel() + 1);

            SceneManager.LoadScene(1);
        } 
        else if (collision.CompareTag("CheckPoint"))
        {
            this.lastCheckPoint = collision.gameObject.transform.position;
        }
        else if (collision.CompareTag("Item"))
        {
            
            CardController card = collision.gameObject.GetComponent<CardController>();

            PlayerController.card = true;

            card.PreDestroy();


        }
    }

    public void TakeDamage(int damage)
    {
        
        audioSource.PlayOneShot(damageSound);

        healthManager.TakeDamage(damage);
    }

    public void TeleportDamage(int damage)
    {
        this.TakeDamage(damage);

        this.transform.position = lastCheckPoint;

        this.rb.linearVelocity = Vector2.zero;
    }

}
