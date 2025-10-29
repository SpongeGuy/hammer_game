using UnityEngine;

public class StayUpright : MonoBehaviour
{
    //used to get the global Rotation
    private float globalXRotation = 0f;
    private float globalYRotation = 0f;
    private float globalZRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        //has is where the global rotation has the top(X axis) facing upward and every other angle follows the pivot point
        transform.eulerAngles = new Vector3(globalXRotation, globalYRotation, globalZRotation);
        transform.rotation = Quaternion.Euler(0f, globalYRotation, globalZRotation);
    }
}
