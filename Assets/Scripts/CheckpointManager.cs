using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance; // Singleton reference

    public Vector2 lastCheckpointPosition; // To store the last checkpoint position
    public GameObject playerPrefab; // Player prefab to respawn
    public Transform respawnPoint; // The spawn point to reset the player position

    private PlayerWeaponSwitcher playerWeaponSwitcher;

    private void Awake()
    {
        // Ensure the instance is set
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Ensure only one CheckpointManager exists
        }

        playerWeaponSwitcher = GameObject.FindObjectOfType<PlayerWeaponSwitcher>();
    }

    // Method to update the last checkpoint position
    public void UpdateCheckpoint(Vector2 newCheckpointPosition)
    {
        lastCheckpointPosition = newCheckpointPosition;
        Debug.Log("Checkpoint position updated to: " + lastCheckpointPosition);
    }

    // Reset the player position, ammo, and enemies
    public void RespawnPlayer()
    {
        if (playerWeaponSwitcher != null)
        {
            // Reset player ammo (can be modified if specific ammo is to be reset)
            playerWeaponSwitcher.ResetAmmo();
        }

        // Reset player position to the last checkpoint
        if (respawnPoint != null)
        {
            respawnPoint.position = lastCheckpointPosition;
        }

        // Reset the enemies
        ResetEnemies();
    }

    // Reset all enemies (You can call specific methods here if needed for enemy reset)
    private void ResetEnemies()
    {
        // Find all enemies and reset them to their initial position and state
        SimpleEnemy[] enemies = FindObjectsOfType<SimpleEnemy>(FindObjectSortMode.None);

        foreach (SimpleEnemy enemy in enemies)
        {
            enemy.ResetEnemy();
        }
    }
}
