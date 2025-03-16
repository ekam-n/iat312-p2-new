using UnityEngine;
using UnityEngine.UI;

public class RespawnerHandler : MonoBehaviour
{
    public Button respawnButton;               // Reference to the respawn button
    public PlayerHealth playerHealth;          // Reference to PlayerHealth (for health reset)
    public PlayerWeaponSwitcher playerWeaponSwitcher;  // Reference to PlayerWeaponSwitcher (for fireball ammo reset)
    public BlowDartWeapon blowDartWeapon;      // Reference to BlowDartWeapon (for dart ammo reset)
    private TikiAmmoPickup[] ammoPickups;      // Array to store all ammo pickups in the scene
    private SimpleEnemy[] enemies;             // Array to store all enemies in the scene

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

        // Find all TikiAmmoPickup objects in the scene
        ammoPickups = FindObjectsOfType<TikiAmmoPickup>();

        // Find all SimpleEnemy objects in the scene
        enemies = FindObjectsOfType<SimpleEnemy>();
    }

    // Called when the respawn button is clicked
    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();  // Handle health respawn (position reset, health restore, etc.)
            
            // Reset fireball ammo
            if (playerWeaponSwitcher != null)
            {
                playerWeaponSwitcher.ResetAmmo();  // Reset all fireball ammo to 0
            }

            // Reset blowdart ammo
            if (blowDartWeapon != null)
            {
                blowDartWeapon.ResetAmmo();  // Reset all dart ammo to 0
            }

            // Reset all ammo pickups in the scene
            if (ammoPickups != null)
            {
                foreach (var ammoPickup in ammoPickups)
                {
                    ammoPickup.ResetPickup();  // Reset the ammo pickup to its initial position
                }
            }

            // Reset all enemies' tranquilized status and positions
            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    enemy.ResetEnemyStatus();  // Reset tranquilization and other statuses
                    enemy.ResetPosition(); 
                    enemy.ResetHealth();    // Reset the enemy's position to its initial spawn position
                }
            }
        }
        else
        {
            Debug.LogError("PlayerHealth reference is missing!");
        }
    }
}
