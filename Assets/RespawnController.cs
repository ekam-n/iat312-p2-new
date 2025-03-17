using UnityEngine;
using UnityEngine.UI;

public class Respawner : MonoBehaviour
{
    public Button respawnButton;              // Reference to the respawn button
    public PlayerHealth playerHealth;         // Reference to PlayerHealth (for health reset)
    public PlayerWeaponSwitcher playerWeaponSwitcher; // Reference to PlayerWeaponSwitcher (for ammo reset)

    void Start()
    {
        // Ensure respawnButton is assigned
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked); // Add listener for button press
        }
        else
        {
            Debug.LogError("Respawn Button is not assigned!");
        }
    }

    // Called when the respawn button is clicked
    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();  // Handle health respawn (position reset, health restore, etc.)
            
            // Reset ammo when the player respawns
            if (playerWeaponSwitcher != null)
            {
                playerWeaponSwitcher.ResetAmmo();  // Reset all ammo to 0
            }
        }
        else
        {
            Debug.LogError("PlayerHealth reference is missing!");
        }
    }
}
