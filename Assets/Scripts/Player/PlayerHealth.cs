using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Player's starting health
    public float respawnDelay = 2f;  // Delay before respawning
    public Vector2 spawnPosition;    // Player's spawn position (2D version)
    public GameObject respawnButton; // Reference to the Respawn Button in the UI

    public float currentHealth;
    public float maxHealth;

    private bool isDead = false;  // Track if the player is dead

    private void Start()
    {
        // Initially set the spawn position to the player's current position at start
        spawnPosition = transform.position;

        // Ensure the respawn button is hidden initially
        respawnButton.SetActive(false);
    }

    // Method to handle damage
    public void TakeDamage(float damage)
    {
        if (isDead) return;  // If the player is dead, don't apply damage

        health -= damage; // Subtract the damage from health

        if (health <= 0)
        {
            Die(); // Call the Die method if health drops to zero
        }
    }

    // Call this method when the player dies (triggered by the KillTile or other logic)
    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            // Disable the player object (or apply death animations)
            gameObject.SetActive(false);

            // Optionally, play a death animation or sound here
            Debug.Log("Player died!");

            // Make the respawn button visible
            respawnButton.SetActive(true);
        }
    }

    // Respawn the player after a delay
    public void Respawn()
    {
        // Use the CheckpointManager to get the last checkpoint position (2D)
        transform.position = CheckpointManager.instance.lastCheckpointPosition;

        Debug.Log("Respawning at position: " + CheckpointManager.instance.lastCheckpointPosition);  // Debug log

        // Optionally, re-enable any components (like Animator or RigidBody2D)
        gameObject.SetActive(true);

        // Reset the player's health
        health = 100f;  // You can reset this to any starting value

        isDead = false;

        Debug.Log("Player respawned!");

        // Hide the respawn button after the respawn
        respawnButton.SetActive(false);
    }

    // Method to update the spawn position when the player reaches a checkpoint
    public void UpdateSpawnPosition(Vector2 newSpawnPosition)
    {
        spawnPosition = newSpawnPosition;
        Debug.Log("Updated spawn position to: " + newSpawnPosition);  // Debug log

        // Update the last checkpoint position in the CheckpointManager
        CheckpointManager.instance.UpdateCheckpoint(newSpawnPosition);
    }
}
