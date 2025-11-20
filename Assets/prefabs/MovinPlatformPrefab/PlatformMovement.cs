using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public bool waitForPlayer = false;
    public float speed;
    public int startingPoint;
    public Transform[] points;
    private int i;

    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        if (!waitForPlayer) 
        {
            transform.position = Vector3.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
        }
        
    }

    
}
