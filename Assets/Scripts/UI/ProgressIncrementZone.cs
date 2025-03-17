using UnityEngine;

public class ProgressIncrementZone : MonoBehaviour
{
    [Tooltip("Total number of zones in this level. Each zone trigger will increment progress by 1 / totalZones.")]
    public int totalZones = 8;

    // Store the player that has triggered this zone (using the root object so that multiple colliders don't cause duplicate increments).
    private GameObject triggeredPlayer = null;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Get the root GameObject of the collider (in case the player has multiple colliders).
        GameObject player = other.transform.root.gameObject;
        if (!player.CompareTag("Player"))
            return;

        // If this player has already triggered the zone, do nothing.
        if (triggeredPlayer == player)
            return;

        // Mark that this player has triggered the zone.
        triggeredPlayer = player;

        // Get the levelProgress component from the scene.
        levelProgress lp = FindAnyObjectByType<levelProgress>();
        if (lp != null)
        {
            // Calculate the increment for each zone.
            float increment = 1f / totalZones;

            // Increase progress by adding the increment.
            lp.progressTracker(increment);
            Debug.Log("Progress increased by " + increment);

            // Check if progress has reached or exceeded 1.
            if (lp.progress >= 1f)
            {
                lp.progress = 0f;
                Debug.Log("Progress reset to 0 after reaching the final zone trigger.");
            }
        }
        else
        {
            Debug.LogWarning("No levelProgress script found in the scene!");
        }
    }
}
