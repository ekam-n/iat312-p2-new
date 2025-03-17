using UnityEngine;

public class Pineapple : MonoBehaviour
{
    [Tooltip("The amount of health to restore when the player picks up this item.")]
    public float healthAmount = 25f;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                // Increase player's health but do not exceed maxHealth.
                playerHealth.currentHealth = Mathf.Min(playerHealth.currentHealth + healthAmount, playerHealth.maxHealth);
                Debug.Log("Pineapple picked up! Player's health is now " + playerHealth.currentHealth);
                
                // Optionally, you can play a sound or animation here before destroying the pickup.
                Destroy(gameObject);
            }
        }
    }
}
