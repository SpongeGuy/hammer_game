using UnityEngine;

public class NailFollow : MonoBehaviour
{
    public int nailNum;
    private float speed;
    private float distance;

    public void OnFollow(int num, float followSpeed, float followDistance)
    {
        nailNum = num;
        speed = followSpeed;
        distance = followDistance;
    }

    // THIS NEEDS A FIX!!!
    // NAILS CURRENTLY ONLY LINE UP IN ONE DIRECTION OF THE PLAYER, INSTEAD OF ROTATING BASED ON PLAYERS DIRECTION.

    // Update is called once per frame
    void Update()
    {
        // Putting extra notes for this since its WIP & I've changed a lot as I've messed with it.
        // Set the player to target.
        Transform player = NailManager.Instance.player;
        // Nails that are collected will line up in a straight line by the player.
        Vector3 desiredPos = player.position - player.forward * (distance * (nailNum + 1));
        // Nail will move to new position.
        transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);
        // Nail will face the player.
        transform.LookAt(player.position);
    }
}
