using UnityEngine;

public class TeleportationTrigger : MonoBehaviour
{
    [Tooltip("Assign the Transform where the player should be teleported to.")]
    public Transform teleportTarget;

    // This method is for 2D collisions; for 3D, use OnTriggerEnter(Collider other)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            if (teleportTarget != null)
            {
                // Teleport the player to the target position.
                other.transform.position = teleportTarget.position;
                Debug.Log("Player teleported to: " + teleportTarget.position);
            }
            else
            {
                Debug.LogWarning("Teleport target not set on " + gameObject.name);
            }
        }
    }
}
