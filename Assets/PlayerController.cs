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

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;

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
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

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

        if (isGrounded && controls.Player.Jump.WasPressedThisFrame())
        {
            float jumpVelocity = Mathf.Sqrt(2f * Physics.gravity.magnitude * maxJumpHeight);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpVelocity, rb.linearVelocity.z);
        }

        if (!isGrounded && controls.Player.Jump.WasReleasedThisFrame() && rb.linearVelocity.y > 0f)
        {
            float lowJumpMultiplier = Mathf.Sqrt(minJumpHeight / maxJumpHeight);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * lowJumpMultiplier, rb.linearVelocity.z);
        }
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

        Vector3 movementDirection = (camForward * moveInput.y) + (camRight * moveInput.x);
        Vector3 movement = movementDirection * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    void OnDisable()
    {
        controls.Disable();
    }

}