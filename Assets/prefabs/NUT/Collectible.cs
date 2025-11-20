using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float collectibleSpeed = 200f;

    // Update is called once per frame

    void Update()
    {
        transform.Rotate(Vector3.up * collectibleSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectibleManager.Instance.CollectNut();
            Destroy(gameObject);
        }
    }
}
