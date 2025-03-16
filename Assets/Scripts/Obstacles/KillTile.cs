using UnityEngine;

public class KillTile : MonoBehaviour
{
    // Called when anything collides with the tile
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Handle player death
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Die();  // Assuming Die() handles the player death logic
            }
        }

        // Check if the object that collided is an enemy
        else if (other.CompareTag("Enemy"))
        {
            // Get the enemy's EnemyBase script and call its Die method
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Die();  // Disable the enemy instead of destroying it
            }
        }
    }
}
