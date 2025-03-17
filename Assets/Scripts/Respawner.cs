using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class RespawnerHandler : MonoBehaviour
{
    public Button respawnButton;               
    public PlayerHealth playerHealth;          
    public BlowDartWeapon blowDartWeapon;      
    private TikiAmmoPickup[] ammoPickups;      
    private AmmoPickup[] molotovPickups;       
    private AmmoPickup[] coconutPickups;       
    private SimpleEnemy[] enemies;             
    private Vector3[] molotovPickupPositions;  
    private Vector3[] coconutPickupPositions;  
    public throwingController playerThrowingController; 

    void Start()
    {
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        }
        else
        {
            Debug.LogError("Respawn Button is not assigned!");
        }

        // Find all ammo pickups
        ammoPickups = FindObjectsOfType<TikiAmmoPickup>();

        // Find all Molotov and Coconut pickups separately
        molotovPickups = FindObjectsOfType<AmmoPickup>().Where(p => p.ammoType == AmmoPickup.AmmoType.Molotov).ToArray();
        coconutPickups = FindObjectsOfType<AmmoPickup>().Where(p => p.ammoType == AmmoPickup.AmmoType.Coconut).ToArray();

        // Store original positions
        molotovPickupPositions = molotovPickups.Select(p => p.transform.position).ToArray();
        coconutPickupPositions = coconutPickups.Select(p => p.transform.position).ToArray();

        // Find all enemies
        enemies = FindObjectsOfType<SimpleEnemy>();
    }

    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();

            if (blowDartWeapon != null)
            {
                blowDartWeapon.ResetAmmo();
            }

            if (ammoPickups != null)
            {
                foreach (var ammoPickup in ammoPickups)
                {
                    ammoPickup.ResetPickup();
                }
            }

            // Reset Molotov Pickups
            for (int i = 0; i < molotovPickups.Length; i++)
            {
                molotovPickups[i].transform.position = molotovPickupPositions[i];
                molotovPickups[i].gameObject.SetActive(true);
            }

            // Reset Coconut Pickups
            for (int i = 0; i < coconutPickups.Length; i++)
            {
                coconutPickups[i].transform.position = coconutPickupPositions[i];
                coconutPickups[i].gameObject.SetActive(true);
            }

            // Reset Enemies
            foreach (var enemy in enemies)
            {
                if (enemy != null && !enemy.gameObject.activeInHierarchy)
                {
                    enemy.gameObject.SetActive(true);
                }
                enemy.ResetEnemyStatus();
                enemy.ResetPosition();
                enemy.ResetHealth();
            }

            // Reset Player Ammo
            if (playerThrowingController != null)
            {
                playerThrowingController.mollyAmmo = 0;  // Reset Molotovs
                playerThrowingController.cocoAmmo = 0;   // Reset Coconuts
            }
        }
        else
        {
            Debug.LogError("PlayerHealth reference is missing!");
        }
    }
}
