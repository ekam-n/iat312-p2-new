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
                // Instead of trying to access a non-existing 'flamethrower' property,
                // we access the flamethrowerInstance from PlayerWeaponSwitcher.
                if (player.IsFlamethrowerEquipped && player.flamethrowerInstance != null)
                {
                    player.AddFireballs(fireballAmount);
                    player.flamethrowerInstance.AddFireballs(fireballAmount); // Add fireballs to the flamethrower
                    Debug.Log("Added fireball ammo to Flamethrower");
                }
                else
                {
                    player.AddFireballs(fireballAmount); // Add ammo to the player if Flamethrower isn't equipped
                    Debug.Log("Added fireball ammo to player");
                }

                Destroy(gameObject); // Destroy the pickup object after collection
            }
        }
    }
}
