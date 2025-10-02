using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public float orbitSpeed = 100f;
    public float minVerticalAngle = -30f;
    public float maxVerticalAngle = 60f;
    public float distance = 5f;

    private Vector2 lookInput;
    private float currentYaw;
    private float currentPitch;

    void Start()
    {
        // initialize camera rotation based on its starting position
        Vector3 direction = transform.localPosition.normalized;
        currentPitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
        currentYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    }

    void LateUpdate()
    {
        
        currentYaw += lookInput.x * orbitSpeed * Time.deltaTime;
        currentPitch -= lookInput.y * orbitSpeed * Time.deltaTime; // Invert Y for intuitive control
        currentPitch = Mathf.Clamp(currentPitch, minVerticalAngle, maxVerticalAngle);

        // calculate camera position using spherical coordinates
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0);
        Vector3 offset = rotation * Vector3.back * distance;
        transform.localPosition = offset;

        // make camera look at the player
        transform.LookAt(transform.parent.position);

        lookInput = Vector2.zero;
    }

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

}