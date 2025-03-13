using UnityEngine;

public class TikiAmmoPickup : MonoBehaviour
{
    public int fireballAmount = 3; // How many fireballs this pickup gives

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwitcher player = other.GetComponent<PlayerWeaponSwitcher>();
            if (player != null)
            {
                // Add fireballs to the player's ammo
                player.AddFireballs(fireballAmount);

                // Destroy the pickup object
                Destroy(gameObject); // Destroy the pickup
            }
        }
    }
}
