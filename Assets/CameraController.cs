using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform player; // Reference to the player GameObject for tracking position
    public PlayerController playerController; // Reference to PlayerController for maxSpeed
    public float orbitAngularSpeed = 125f; // Speed of manual camera orbit via right stick
    public float minOrbitPitch = -30f; // Minimum pitch angle for camera orbit
    public float maxOrbitPitch = 60f; // Maximum pitch angle for camera orbit
    public float orbitDistance = 10f; // Distance from camera target to camera
    public float orbitInputSmoothTime = 0.008f; // Smoothing time for orbit input
    public float cameraPositionSmoothTime = 0.08f; // Smoothing time for camera position following
    public float targetPositionSmoothTime = 0.2f; // Smoothing time for camera target following player
    public float autoOrbitAngularSpeed = 0.5f; // Base speed of auto-orbit based on player movement
    public float autoOrbitFadeTime = 1.5f; // Time to fade auto-orbit in/out when toggling manual input
    public float autoOrbitMovementSmoothTime = 0.3f; // Smoothing time for auto-orbit movement response

    private Vector2 rawOrbitInput; // Raw input from right stick for orbit control
    private Vector2 smoothedOrbitInput; // Smoothed input for orbit positioning
    private Vector2 orbitInputSmoothVelocity; // Velocity for orbit input smoothing
    private Vector3 cameraPositionSmoothVelocity; // Velocity for camera position smoothing
    private Vector3 targetPositionSmoothVelocity; // Velocity for camera target smoothing
    private Vector3 previousPlayerPosition; // Last frame's player position to calculate movement
    private bool isManualOrbitActive; // True when right stick input is detected
    private float autoOrbitStrength; // 0 to 1, scales auto-orbit strength during fade
    private Vector3 smoothedPlayerMovementDelta; // Smoothed player movement for auto-orbit
    private Vector3 playerMovementSmoothVelocity; // Velocity for player movement smoothing
    private Vector3 cameraTargetPosition; // Smoothed target position for camera to orbit and look at

    private float orbitYawAngle; // Current horizontal orbit angle (degrees)
    private float orbitPitchAngle; // Current vertical orbit angle (degrees)

    void Start()
    {
        // Initialize the camera's state and ensure references are set
        if (player == null)
        {
            Debug.LogError("Player transform not assigned in CameraController!");
            return;
        }
        if (playerController == null)
        {
            Debug.LogError("PlayerController not assigned in CameraController!");
            return;
        }
        // Initialize camera orbit angles based on its starting position relative to player
        Vector3 direction = (transform.position - player.position).normalized;
        orbitPitchAngle = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        orbitYawAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        smoothedOrbitInput = Vector2.zero;
        previousPlayerPosition = player.position; // Initialize last position
        autoOrbitStrength = 1f; // Auto-orbit starts at full strength
        smoothedPlayerMovementDelta = Vector3.zero; // Initialize smoothed movement
        cameraTargetPosition = player.position; // Initialize target at player position
    }

    void LateUpdate()
    {
        // Update camera position and orbit each frame, skip if references are missing
        if (player == null || playerController == null) return;
        HandleCameraMovement();
    }

    // Orchestrates camera updates by calling modular functions in sequence
    private void HandleCameraMovement()
    {
        // Process orbit input, update orbit angles, set position, and orient camera
        SmoothLookInput();
        UpdateCameraRotation();
        HandleAutoRotation();
        UpdateCameraTarget();
        SetCameraPosition();
        OrientCameraToPlayer();
        ResetLookInput();
    }

    // Smooths right stick input for fluid orbit positioning
    private void SmoothLookInput()
    {
        // Apply SmoothDamp to orbit input for smooth positioning
        smoothedOrbitInput = Vector2.SmoothDamp(smoothedOrbitInput, rawOrbitInput, ref orbitInputSmoothVelocity, orbitInputSmoothTime);
    }

    // Updates camera yaw and pitch for orbit position based on right stick input
    private void UpdateCameraRotation()
    {
        // Use raw input for immediate manual orbit updates; smoothed for auto-orbit
        Vector2 inputForOrbit = isManualOrbitActive ? rawOrbitInput : smoothedOrbitInput;
        orbitYawAngle += inputForOrbit.x * orbitAngularSpeed * Time.deltaTime;
        orbitPitchAngle -= inputForOrbit.y * orbitAngularSpeed * Time.deltaTime; // Invert Y for intuitive control
        // Clamp pitch to prevent extreme vertical angles
        orbitPitchAngle = Mathf.Clamp(orbitPitchAngle, minOrbitPitch, maxOrbitPitch);
    }

    // Updates the camera target position to smoothly follow the player
    private void UpdateCameraTarget()
    {
        // Smoothly move the camera target toward the player position for positional play
        cameraTargetPosition = Vector3.SmoothDamp(cameraTargetPosition, player.position, ref targetPositionSmoothVelocity, targetPositionSmoothTime);
    }

    // Handles automatic camera orbit based on smoothed player movement and actual speed
    private void HandleAutoRotation()
    {
        // Update auto-orbit strength based on manual input (0 if active, 1 if inactive)
        float targetStrength = isManualOrbitActive ? 0f : 1f;
        autoOrbitStrength = Mathf.Lerp(autoOrbitStrength, targetStrength, Time.deltaTime / autoOrbitFadeTime);

        // Calculate raw player movement from position change
        Vector3 playerPositionDelta = player.position - previousPlayerPosition;

        // Smooth the movement vector to reduce jitter from micro-movements
        smoothedPlayerMovementDelta = Vector3.SmoothDamp(smoothedPlayerMovementDelta, playerPositionDelta, ref playerMovementSmoothVelocity, autoOrbitMovementSmoothTime);

        // Apply auto-orbit if movement is significant and auto-orbit is active
        if (smoothedPlayerMovementDelta.magnitude > 0.01f && autoOrbitStrength > 0f)
        {
            // Project smoothed movement and camera forward onto XZ plane
            Vector3 moveDirection = smoothedPlayerMovementDelta.normalized;
            moveDirection.y = 0;
            moveDirection.Normalize();

            Vector3 cameraForward = transform.forward;
            cameraForward.y = 0;
            cameraForward.Normalize();

            // Calculate angle between smoothed movement and camera forward
            float angle = Vector3.SignedAngle(cameraForward, moveDirection, Vector3.up);
            // Scale orbit by angle (max at 90°, zero at 0°/180°)
            float rotationFactor = Mathf.Abs(Mathf.Sin(angle * Mathf.Deg2Rad)); // 0 to 1, peaks at 90°
            // Scale by actual movement speed (displacement per frame) relative to maxSpeed
            float speedFactor = (smoothedPlayerMovementDelta.magnitude / Time.deltaTime) / playerController.MaxSpeed;
            // Clamp speedFactor to prevent excessive orbit speed
            speedFactor = Mathf.Clamp(speedFactor, 0f, 1f);
            float autoOrbit = angle * rotationFactor * autoOrbitAngularSpeed * autoOrbitStrength * speedFactor * Time.deltaTime;
            orbitYawAngle += autoOrbit;
        }

        // Update last player position for next frame
        previousPlayerPosition = player.position;
    }

    // Sets camera position to orbit the smoothed camera target with smooth following
    private void SetCameraPosition()
    {
        // Calculate desired position using spherical coordinates relative to camera target
        Quaternion rotation = Quaternion.Euler(orbitPitchAngle, orbitYawAngle, 0);
        Vector3 offset = rotation * Vector3.back * orbitDistance;
        Vector3 desiredPosition = cameraTargetPosition + offset;

        // Smoothly move camera to desired position for additional positional lag
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref cameraPositionSmoothVelocity, cameraPositionSmoothTime);
    }

    // Orients camera to face the smoothed camera target for consistent positional play
    private void OrientCameraToPlayer()
    {
        // Make camera look at the camera target position to align with orbit smoothing
        transform.LookAt(cameraTargetPosition);
    }

    // Resets orbit input to prevent continuous orbiting
    private void ResetLookInput()
    {
        rawOrbitInput = Vector2.zero;
    }

    // Receives and processes right stick input for orbit control
    public void SetLookInput(Vector2 input)
    {
        rawOrbitInput = input;
        // Detect right stick activity with a threshold to avoid noise
        isManualOrbitActive = rawOrbitInput.magnitude > 0.1f;
    }
}