using UnityEngine;

public class TikiAmmoPickup : MonoBehaviour
{
    public int fireballAmount = 3;    // Ammo for fireballs
    public int normalDartAmount = 5;  // Ammo for normal darts
    public int poisonDartAmount = 3;  // Ammo for poison darts
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;  // Store the original position of the pickup
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwitcher playerWeaponSwitcher = other.GetComponent<PlayerWeaponSwitcher>();
            BlowDartWeapon blowDartWeapon = other.GetComponent<BlowDartWeapon>();
            
            if (playerWeaponSwitcher != null)
            {
                // Add fireball ammo
                playerWeaponSwitcher.AddFireballs(fireballAmount);
            }

            if (blowDartWeapon != null)
            {
                // Add blowdart ammo (both normal and poison darts)
                blowDartWeapon.AddNormalDarts(normalDartAmount);
                blowDartWeapon.AddPoisonDarts(poisonDartAmount);
            }

            gameObject.SetActive(false);  // Disable the ammo pickup after it's collected
        }
    }

    // Reset the ammo pickup's position and make it active again
    public void ResetPickup()
    {
        transform.position = initialPosition;  // Reset the position
        gameObject.SetActive(true);            // Reactivate the ammo pickup
    }
}
