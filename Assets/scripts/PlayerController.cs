using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Settings")]
    public float maxSpeed = 10f; // Max movement speed
    public float accelTimeGround = 0.075f; // Time to reach max speed
    public float accelTimeAir = 0.15f; // Time to reach max speed in mid-air
    public float decelTime = 0.2f; // Time to stop
    public float gravity = -27.5f; // Gravity strength
    public float maxJumpHeight = 3f; // Max jump height when holding jump button
    public float minJumpHeight = 0.2f; // Min jump height when releasing jump button early
    public float jumpBufferTime = 0.2f; // Time (seconds) to buffer jump input before landing



    private CharacterController controller;
    private Vector3 velocity; // Current movement velocity
    private Vector3 targetVelocity; // Target velocity for horizontal movement smoothing
    private Vector3 velocitySmoothVelocity; // Velocity for SmoothDamp interpolation
    private float currentSpeed; // Current speed magnitude
    private bool isGrounded;
    private float accelTime;
    private float jumpBufferTimer = 0f; // Timer for buffered jump input

    public float MaxSpeed => maxSpeed; // Expose maxSpeed

    [Header("Camera Settings")]
    public Transform cameraTransform; // camera ref
    private CameraController cameraController;
    private PlayerControls controls;

    void Awake()
    {
        // instead of a controls component, we're using the c# class instead
        // so it has to be enabled first
        controls = new PlayerControls();
        controls.Enable();
    }

    void Start()
    {
        // setup CharacterController
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component not found on Player!");
        }

        // camera setup
        if (cameraTransform != null)
        {
            cameraController = cameraTransform.GetComponent<CameraController>();
            if (cameraController == null)
            {
                Debug.LogError("CameraController script not found on Camera child!");
            }
        }
        else
        {
            Debug.LogError("Camera child not found on Player!");
        }

        targetVelocity = Vector3.zero;
        velocitySmoothVelocity = Vector3.zero;
        accelTime = accelTimeGround;
    }

    void Update()
    {
        // Read input
        Vector2 moveInput = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 lookInput = controls.Player.Look.ReadValue<Vector2>();

        // Pass look input to CameraController
        if (cameraController != null)
        {
            cameraController.SetLookInput(lookInput);
        }

        // Update ground state
        UpdateGroundState();

        // Handle core movement
        HandleJump();
        Vector3 moveDirection = GetCameraRelativeDirection(moveInput);
        UpdateHorizontalSpeed(moveInput);
        HandleAbilities(); // Placeholder for future abilities (hammer swing, special jumps)
        ApplyGravity();
        ApplyMovement(moveDirection);
        HandleWallJump(); // Placeholder for future wall jumping
        HandleLedgeGrab(); // Placeholder for future ledge grabbing
    }

    // Updates ground state using CharacterController
    private void UpdateGroundState()
    {
        isGrounded = controller.isGrounded;
    }

    // Handles jump input and applies variable jump height
    private void HandleJump()
    {
        // Buffer jump input if pressed (even in air)
        if (controls.Player.Jump.WasPressedThisFrame())
        {
            jumpBufferTimer = jumpBufferTime;
        }

        // Decrement buffer timer
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }

        // Start jump when grounded and buffer is active or jump is pressed
        if (isGrounded && jumpBufferTimer > 0f)
        {
            // Execute buffered jump
            float jumpVelocity = Mathf.Sqrt(2f * Mathf.Abs(gravity) * maxJumpHeight);
            velocity.y = jumpVelocity;
            jumpBufferTimer = 0f; // Clear buffer
        }

        // Reduce jump height if jump button is released early
        if (!isGrounded && controls.Player.Jump.WasReleasedThisFrame() && velocity.y > 0f)
        {
            // Scale velocity to achieve minJumpHeight
            float lowJumpMultiplier = Mathf.Sqrt(minJumpHeight / maxJumpHeight);
            velocity.y *= lowJumpMultiplier;
        }
    }

    // Gets camera-relative movement direction from input
    private Vector3 GetCameraRelativeDirection(Vector2 moveInput)
    {
        Vector3 moveDirection = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 camForward = cameraTransform.forward;
            camForward.y = 0f;
            camForward.Normalize();

            Vector3 camRight = cameraTransform.right;
            camRight.y = 0f;
            camRight.Normalize();

            moveDirection = (camForward * moveInput.y) + (camRight * moveInput.x);
        }
        return moveDirection;
    }

    

    // Placeholder for future abilities (e.g., hammer swing, special jump parameters, speedup/skid)
    private void HandleAbilities()
    {
        // TODO: Implement hammer swing, special jumps
    }

    // Applies gravity to vertical velocity
    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    // Updates horizontal speed with acceleration/deceleration smoothing
    private void UpdateHorizontalSpeed(Vector2 moveInput)
    {
        if (isGrounded)
        {
            accelTime = accelTimeGround;
        }
        else
        {
            accelTime = accelTimeAir;
        }

        // Prevent deceleration in mid-air when no input
        if (!isGrounded && moveInput.magnitude == 0f)
        {
            return; // Preserve currentSpeed
        }

        float targetSpeed = moveInput.magnitude * maxSpeed;
        float smoothTime = (targetSpeed > currentSpeed) ? accelTime : decelTime;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime / smoothTime);
    }

    // Applies horizontal velocity and moves with CharacterController
    private void ApplyMovement(Vector3 moveDirection)
    {
        targetVelocity = moveDirection * currentSpeed;

        // Preserve direction and speed in mid-air when no input
        if (!isGrounded && moveDirection == Vector3.zero)
        {
            targetVelocity = new Vector3(velocity.x, 0f, velocity.z).normalized * currentSpeed;
        }

        // Smoothly interpolate horizontal velocity toward target velocity
        float smoothTime = (targetVelocity.magnitude > velocity.magnitude) ? accelTime : decelTime;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
        horizontalVelocity = Vector3.SmoothDamp(horizontalVelocity, targetVelocity, ref velocitySmoothVelocity, smoothTime);
        velocity.x = horizontalVelocity.x;
        velocity.z = horizontalVelocity.z;

        // Reset vertical velocity when grounded (small downward force to stay grounded)
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Apply air control limitation (placeholder for future mid-air velocity alteration)
        HandleAirControl();

        // Move with CharacterController
        controller.Move(velocity * Time.deltaTime);
    }

    // Placeholder for limiting player velocity alteration in mid-air
    private void HandleAirControl()
    {
        // TODO: Implement reduced air control (e.g., scale horizontal velocity when !isGrounded)
        // Example: if (!isGrounded) { velocity.x *= 0.5f; velocity.z *= 0.5f; }
    }

    // Placeholder for wall jumping mechanics
    private void HandleWallJump()
    {
        // TODO: Implement wall jump detection and logic
    }

    // Placeholder for ledge grabbing mechanics
    private void HandleLedgeGrab()
    {
        // TODO: Implement ledge grab detection and logic
    }

    void OnDisable()
    {
        controls.Disable();
    }
}