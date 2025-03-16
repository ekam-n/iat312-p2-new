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

    public void RespawnPlayer()
    {
    // Reset player position to the last checkpoint
    if (respawnPoint != null)
    {
        respawnPoint.position = lastCheckpointPosition;
    }

    // Reset player ammo and weapons
    if (playerWeaponSwitcher != null)
    {
        playerWeaponSwitcher.ResetAmmo();  // Reset ammo to 0
    }

    // Optionally, you can reinitialize the player state, equip weapons, etc.
    // E.g., set the player to a default state if needed (like equipping the first weapon).
    }

}
