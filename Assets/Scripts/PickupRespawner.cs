using UnityEngine;

public class PickupRespawner : MonoBehaviour
{
    [Tooltip("List of pickup GameObjects that should be respawned when the player respawns.")]
    public GameObject[] pickupsToRespawn;

    /// <summary>
    /// Call this method (e.g., from your respawn handler) to reset all pickups.
    /// </summary>
    public void ResetAllPickups()
    {
        foreach (GameObject pickup in pickupsToRespawn)
        {
            if (pickup != null)
            {
                // Call ResetPickup on the pickup, if it has one.
                // Using SendMessage allows different pickup scripts to respond to this method.
                pickup.SendMessage("ResetPickup", SendMessageOptions.DontRequireReceiver);
                // Ensure the pickup is active.
                pickup.SetActive(true);
                Debug.Log("Pickup " + pickup.name + " has been reset.");
            }
        }
    }
}
