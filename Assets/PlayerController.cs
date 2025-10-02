using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxJumpHeight = 1f;
    public float minJumpHeight = 0.2f;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float accelSmoothTime = 0.2f; // time to reach max speed with sqrt curve
    public float decelSmoothTime = 0.3f; // time to decelerate to zero linearly

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 smoothedMoveInput;
    private bool isGrounded;

    // smoothing moving state
    private float currentMagnitude = 0f;
    private bool isAccelerating = false;
    private float accelStartTime;
    private float accelConstant; // precomputed a = 1 / sqrt(accelSmoothTime)
    private float decelRate; // precomputed 1 / decelSmoothTime

    // variables needed for camera
    private CameraController cameraController;
    private PlayerControls controls;
    private Transform camTransform;

    

    void Awake()
    {
        // instead of a controls component, we're using the c# class instead
        // so it has to be enabled first
        controls = new PlayerControls();
        controls.Enable();
    }

    void Start()
    {
        // rigidbody and make sure it doesn't topple over awkwardly
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        // precompute constants
        accelConstant = 1f / Mathf.Sqrt(accelSmoothTime);
        decelRate = 1f / decelSmoothTime;

        // camera setup
        Transform cameraTransform = transform.Find("Camera");
        if (cameraTransform != null)
        {
            camTransform = cameraTransform;
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
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        moveInput = controls.Player.Movement.ReadValue<Vector2>();

        Vector2 lookInput = controls.Player.Look.ReadValue<Vector2>();

        if (cameraController != null)
        {
            cameraController.SetLookInput(lookInput);
        }

        SmoothMovementInput();

        // jump start
        if (isGrounded && controls.Player.Jump.WasPressedThisFrame())
        {
            float jumpVelocity = Mathf.Sqrt(2f * Physics.gravity.magnitude * maxJumpHeight);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }
        // variable jump / jump button was let go of
        if (!isGrounded && controls.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            float lowJumpMultiplier = Mathf.Sqrt(minJumpHeight / maxJumpHeight);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * lowJumpMultiplier, rb.linearVelocity.z);
        }
    }

    private void SmoothMovementInput()
    {
        float targetMag = moveInput.magnitude;
        Vector2 targetDir = moveInput.normalized;
        print(isAccelerating);

        if (targetMag > 0f)
        {
            // acceleration phase: sqrt curve
            if (!isAccelerating)
            {
                isAccelerating = true;
                accelStartTime = Time.time;
                currentMagnitude = 0f; // reset magnitude when starting acceleration
            }

            float elapsed = Time.time - accelStartTime;
            float curveMag = accelConstant * Mathf.Sqrt(elapsed);
            currentMagnitude = Mathf.Min(curveMag, targetMag);
        }
        else
        {
            // deceleration phase: linear
            if (isAccelerating)
            {
                isAccelerating = false;
            }

            currentMagnitude -= decelRate * Time.deltaTime;
            currentMagnitude = Mathf.Max(currentMagnitude, 0f);
        }

        // Apply smoothed magnitude to target direction
        smoothedMoveInput = targetDir * currentMagnitude;
    }

    void FixedUpdate()
    {
        if (camTransform == null) return;

        Vector3 camForward = camTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = camTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 movementDirection = (camForward * smoothedMoveInput.y) + (camRight * smoothedMoveInput.x);

        Vector3 movement = movementDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnDisable()
    {
        controls.Disable();
    }

}