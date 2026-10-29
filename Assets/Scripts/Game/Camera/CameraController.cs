using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.3f;

    [SerializeField] private InputActionReference look;

    private Vector3 velocity;

    private void OnEnable()
    {
        look.action.Enable();

        look.action.performed += OnLook;
        look.action.canceled  += OnLookCanceled;
    }

    private void OnDisable()
    {
        look.action.performed -= OnLook;
        look.action.canceled -= OnLookCanceled;

        look.action.Disable();
    }

    private void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref velocity, smoothTime);
    }

    private void OnLook(InputAction.CallbackContext context)
    {

        float lookingAt = look.action.ReadValue<float>();


        if (lookingAt > 0)
        {
            offset = new Vector3(5, 5, -10);
        } 
        else if (lookingAt < 0)
        {
            offset = new Vector3(5, -3, -10);
        }

    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        offset = new Vector3(5, 2, -10);
    }
}
