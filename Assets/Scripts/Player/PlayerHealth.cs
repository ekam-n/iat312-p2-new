using UnityEngine;
using UnityEngine.UI; // Import to handle UI elements

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f; // Player's starting health
    public float respawnDelay = 2f;  // Delay before respawning
    public Vector3 spawnPosition;    // Player's spawn position
    public GameObject respawnButton; // Reference to the Respawn Button in the UI

    private bool isDead = false;  // Track if the player is dead

    private void Start()
    {
        // Set the spawn position (this can be adjusted as needed)
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

            // Optionally, play a death animation or sound here

            // Trigger the respawn after a delay if needed (you can manually respawn from the UI button too)
            // Invoke(nameof(Respawn), respawnDelay);
        }
    }

    // Respawn the player after a delay
    public void Respawn()
    {
        // Reset the player's position to the spawn point
        transform.position = spawnPosition;

        // Optionally, re-enable any components (like Animator or RigidBody)
        gameObject.SetActive(true);

        // Reset the player's health
        health = 100f;  // You can reset this to any starting value

        isDead = false;

        Debug.Log("Player respawned!");

        // Hide the respawn button after the respawn
        respawnButton.SetActive(false);
    }
}
