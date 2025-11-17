using UnityEngine;

public class NailCollect : MonoBehaviour
{
    // On collision with player, use CollectNail in NailManager, and remove the collider.
    // I removed the collider so that the nail cant be collected again.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NailManager.Instance.CollectNail(gameObject);
            Collider col = GetComponent<Collider>();
            if (col) col.enabled = false;
        }
    }
}
