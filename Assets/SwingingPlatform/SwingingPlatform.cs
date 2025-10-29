using UnityEngine;

public class SwingingPlatform : MonoBehaviour
{
    //startingPoint is actually endingPoint (it starts at the end point)
    [SerializeField] private float startingPoint = 45f;
    //think of this as you got two points starting and ending and then the amount of degrees between them
    [SerializeField] private float degreesOfMovement = -45f;
    //speed of rotation
    [SerializeField] private float speed = 0f;
    //used to get the starting Rotation
    private float startYRotation = 0f;
    private float startZRotation = 0f;
    //temp number is also used to get middle of roation points
    private float tempX = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.eulerAngles = new Vector3(tempX, startYRotation, startZRotation);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.PingPong(Time.time * speed, degreesOfMovement) - startingPoint;
        transform.rotation = Quaternion.Euler(angle, startYRotation, startZRotation);
    }
}
