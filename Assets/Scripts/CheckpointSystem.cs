using UnityEngine;
using System.Collections;

public class CheckpointSystem : MonoBehaviour
{
    [Header("Player References")]
    public Transform player;
    public PlayerHealth playerHealth;  // Changed from Health to PlayerHealth
    
    [Header("Checkpoint Settings")]
    private Vector3 respawnPoint;
    public float respawnDelay = 2f;
    public GameObject respawnEffect;
    
    [Header("Player Death")]
    public GameObject deathEffect;
    
    private void Start()
    {
        // Set initial respawn point to player's starting position
        respawnPoint = player.position;
        
        // Subscribe to player death event
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied += HandlePlayerDeath;
        }
    }
    
    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        if (playerHealth != null)
        {
            playerHealth.OnPlayerDied -= HandlePlayerDeath;
        }
    }
    
    // Call this when player dies
    public void HandlePlayerDeath()
    {
        // Show death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, player.position, Quaternion.identity);
        }
        
        // Player is already disabled in PlayerHealth.Die()
        
        // Start respawn sequence
        StartCoroutine(RespawnAfterDelay());
    }
    
    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        
        // Move player to last checkpoint
        player.position = respawnPoint;
        
        // Show respawn effect
        if (respawnEffect != null)
        {
            Instantiate(respawnEffect, respawnPoint, Quaternion.identity);
        }
        
        // Reset player health
        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }
        
        // Show player
        player.gameObject.SetActive(true);
    }
    
    // Update the respawn point when player reaches a checkpoint
    public void SetCheckpoint(Vector3 newCheckpointPosition)
    {
        respawnPoint = newCheckpointPosition;
        Debug.Log("Checkpoint set at: " + respawnPoint);
    }
}