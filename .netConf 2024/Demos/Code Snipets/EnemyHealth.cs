using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public GameManager gameManager; // Reference to GameManager

    public int maxHealth = 100;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        gameManager = FindFirstObjectByType<GameManager>(); // Find GameManager in the scene
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(20);
            collision.gameObject.SetActive(false); // Deactivates bullet on impact
        }
    }

    void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            gameManager.IncreaseScore(10); // Add score when enemy is defeated
            Destroy(gameObject);
        }
    }
}


