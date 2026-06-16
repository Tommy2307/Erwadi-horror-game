using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 5.0f;
    [SerializeField] private float sprintSpeed = 8.5f; 
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 1.5f;

    [Header("Look Settings")]
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float mouseSensitivity = 0.1f;
    [SerializeField] private float upperLookLimit = -80f;
    [SerializeField] private float lowerLookLimit = 80f;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation = 0f;
    private bool isSprinting = false; 

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock the cursor to the center of the game screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerCamera == null)
        {
            playerCamera = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    // 1. INPUT SYSTEM CALLBACKS
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    // 2. MOVEMENT CALCULATIONS
    private void HandleMovement()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // Translate inputs relative to facing direction
        Vector3 moveDirection = transform.forward * moveInput.y + transform.right * moveInput.x;

        // Direct Keyboard evaluation to completely avoid toggle issues or event misfires
        if (Keyboard.current != null)
        {
            isSprinting = Keyboard.current.leftShiftKey.isPressed;
        }

        // Dynamically select speed based on player's sprint switch flag
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

        // Apply down-force physics over frame time
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    // 3. CAMERA ROTATION CALCULATIONS
    private void HandleMouseLook()
    {
        if (playerCamera == null) return;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, upperLookLimit, lowerLookLimit);
        playerCamera.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);
    }
}