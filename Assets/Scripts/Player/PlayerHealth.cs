using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Player's starting health
    public float maxHealth = 100f; // Player's maximum health
    public float respawnDelay = 2f;  // Delay before respawning
    public Vector2 spawnPosition;    // Player's spawn position (2D version)
    public GameObject respawnButton; // Reference to the Respawn Button in the UI

    private bool isDead = false;  // Track if the player is dead
    private int killCount = 0;
    private bool hasTakenDamage = false;

    public Image healthBar;
    
    private void Start()
    {
        // Initially set the spawn position to the player's current position at start
        spawnPosition = transform.position;

        // Ensure the respawn button is hidden initially
        respawnButton.SetActive(false);
    }
    public void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    // Method to handle damage
    public void TakeDamage(float damage)
    {
        if (isDead) return;  // If the player is dead, don't apply damage

        health -= damage; // Subtract the damage from health
        hasTakenDamage = true;

        if (health <= 0)
        {
            Die(); // Call the Die method if health drops to zero
            healthBar.fillAmount = Mathf.Clamp(0 / maxHealth, 0, 1);
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
        Vector2 respawnPos = CheckpointManager.instance.lastCheckpointPosition;
        // If lastCheckpointPosition is uninitialized (e.g. Vector2.zero) and that’s not a valid checkpoint,
        // fall back to the player's initial spawn position.
        if(respawnPos == Vector2.zero)
        {
            respawnPos = spawnPosition;
        }
        
        transform.position = respawnPos;
        Debug.Log("Respawning at position: " + respawnPos);

        // Re-enable components and reset health...
        gameObject.SetActive(true);
        health = 100f;
        isDead = false;
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
    public void IncrementKillCount()
    {
        if (hasTakenDamage) return;

        killCount++;
        if (killCount >= 3)
        {
            IncreaseHealth();
            killCount = 0;
        }
    }

    private void IncreaseHealth()
    {
        health = Mathf.Min(health + 10, 100);
    }
}
