using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public enum AmmoType { Molotov, Coconut }
    public AmmoType ammoType;
    public int ammoAmount = 5; // Adjust this per pickup

    private Vector3 initialPosition; // Store the initial position

    private void Start()
    {
        initialPosition = transform.position; // Save the spawn position
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        throwingController player = collision.GetComponent<throwingController>();

        if (player != null)
        {
            if (ammoType == AmmoType.Molotov && player.mollyAmmo < player.maxMolly)
            {
                player.mollyAmmo = Mathf.Min(player.mollyAmmo + ammoAmount, player.maxMolly);
            }
            else if (ammoType == AmmoType.Coconut && player.cocoAmmo < player.maxCoco)
            {
                player.cocoAmmo = Mathf.Min(player.cocoAmmo + ammoAmount, player.maxCoco);
            }

            // Instead of destroying, disable it
            gameObject.SetActive(false);
        }
    }

    // Method to reset the ammo pickup
    public void ResetPickup()
    {
        transform.position = initialPosition; // Reset to original position
        gameObject.SetActive(true); // Reactivate the object
    }
}
