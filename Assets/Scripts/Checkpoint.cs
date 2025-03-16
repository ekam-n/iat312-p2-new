using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the checkpoint
        if (other.CompareTag("Player"))
        {
            // Update the spawn position to the checkpoint's position
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.UpdateSpawnPosition(transform.position);
                Debug.Log("Checkpoint reached! Spawn position updated to: " + transform.position);  // Debug log
            }

            // Update the checkpoint position in the CheckpointManager
            CheckpointManager.instance.UpdateCheckpoint(transform.position);
        }
    }
}
