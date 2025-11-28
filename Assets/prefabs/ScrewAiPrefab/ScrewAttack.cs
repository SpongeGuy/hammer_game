using UnityEngine;

public class ScrewAttack : MonoBehaviour
{
    [SerializeField] string playerTag = "Player";
    [SerializeField] Transform screw;
    public float pushForce = 1f;
    private bool onCooldown = false;
    public float cooldownDuration = 5.0f; // The total time for the cooldown in seconds
    private float currentCooldown = 0f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            if (onCooldown == false)
            {
                Vector3 pushDirection = screw.position - other.transform.position;
                pushDirection.y = 0;
                pushDirection.Normalize();
                other.GetComponent<CharacterController>().Move(-pushDirection * pushForce);
                currentCooldown = cooldownDuration;
                PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage();
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(playerTag))
        {
            
        }    
    }

    void Update()
    {
        if (currentCooldown > 0)
        {
            onCooldown = true;
            currentCooldown -= Time.deltaTime;
            // Ensure cooldown doesn't go below zero
            if (currentCooldown < 0)
            {
                currentCooldown = 0;
            }
        }
        if(onCooldown == true && currentCooldown == 0)
        {
            onCooldown = false;
        }
    }
}
