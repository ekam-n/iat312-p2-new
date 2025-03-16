using UnityEngine;

public class KillTile : MonoBehaviour
{
    // Called when the player collides with the tile
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call your player health or death logic here
            // For example, if you have a PlayerHealth script:
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Die();  // Assuming Die() handles death logic
            }
        }
    }
}

