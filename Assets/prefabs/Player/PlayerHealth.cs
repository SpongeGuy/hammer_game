using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance;
    public int maxHealth = 3;
    private int currentHealth;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    private bool isInvincible = false;
    public float invincibleTime = 2f;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage()
    {
        if (isInvincible == true) {
            return;
        }
        currentHealth -= 1;
        Debug.Log("Player took damage. Current Health: " + currentHealth);
        if (currentHealth <= 2)
        {
            heart1.SetActive(false);
        }
        if (currentHealth <=1)
        {
            heart2.SetActive(false);
        }
        if (currentHealth <=0)
        {
            heart3.SetActive(false);
        }
        
        if (currentHealth <= 0)
        {
            Die();
        } else {
            isInvincible = true;
            Invoke("NoMoreInvincible", 1f);
        }
    }

    void Die()
    {
        Menus.Instance.GameOver();
    }

    void NoMoreInvincible() {
        isInvincible = false;
    }
}
