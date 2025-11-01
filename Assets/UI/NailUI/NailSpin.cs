using UnityEngine;

public class NailSpin : MonoBehaviour
{
    public float spinSpeed = 200f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * spinSpeed * Time.deltaTime, Space.World);
    }
}
