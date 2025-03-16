using UnityEngine;

public class BlowdartAmmo : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool wasCollected;

    void Start()
    {
        // Store the initial position
        originalPosition = transform.position;
        wasCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeaponSwitcher playerWeaponSwitcher = other.GetComponent<PlayerWeaponSwitcher>();
            if (playerWeaponSwitcher != null)
            {
                if (CompareTag("NormalDartAmmo"))
                {
                    playerWeaponSwitcher.AddNormalDarts(5);
                }
                else if (CompareTag("PoisonDartAmmo"))
                {
                    playerWeaponSwitcher.AddPoisonDarts(2);
                }
            }
            
            wasCollected = true;
            gameObject.SetActive(false); // Hide the pickup
        }
    }

    // Reset the ammo pickup when the player respawns
    public void ResetPickup()
    {
        transform.position = originalPosition; // Reset position
        gameObject.SetActive(true); // Make it visible again
        wasCollected = false;
    }
}