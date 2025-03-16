using UnityEngine;

public class TeleportationTrigger : MonoBehaviour
{
    [Tooltip("Assign the Transform for the Next Zone Teleport (for left or bottom exits)")]
    public Transform nextZoneTeleportTarget;
    
    [Tooltip("Assign the Transform for the Previous Zone Teleport (for right or top exits)")]
    public Transform previousZoneTeleportTarget;
    
    [Tooltip("If true, the teleport behavior is reversed (left/bottom exit go to previous zone and right/top exit go to next zone)")]
    public bool reverseZones = false;

    // Use OnTriggerExit2D to detect when the player leaves the trigger area.
    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object is the player.
        if (other.CompareTag("Player"))
        {
            // Use the trigger's position as the center.
            Vector2 center = (Vector2)transform.position;
            // Get the player's exit position.
            Vector2 exitPos = other.transform.position;

            // Determine if the player exited left or below the trigger center.
            bool exitLeftOrBottom = exitPos.x < center.x || exitPos.y < center.y;

            if (!reverseZones)
            {
                // Default behavior: left/bottom exits go to next zone; right/top exits go to previous zone.
                if (exitLeftOrBottom)
                {
                    if (nextZoneTeleportTarget != null)
                    {
                        other.transform.position = nextZoneTeleportTarget.position;
                        Debug.Log("Player teleported to next zone at: " + nextZoneTeleportTarget.position);
                    }
                    else
                    {
                        Debug.LogWarning("Next zone teleport target not set on " + gameObject.name);
                    }
                }
                else // Exiting right or top
                {
                    if (previousZoneTeleportTarget != null)
                    {
                        other.transform.position = previousZoneTeleportTarget.position;
                        Debug.Log("Player teleported to previous zone at: " + previousZoneTeleportTarget.position);
                    }
                    else
                    {
                        Debug.LogWarning("Previous zone teleport target not set on " + gameObject.name);
                    }
                }
            }
            else // reverseZones is true; swap the behavior.
            {
                if (exitLeftOrBottom)
                {
                    if (previousZoneTeleportTarget != null)
                    {
                        other.transform.position = previousZoneTeleportTarget.position;
                        Debug.Log("Player teleported (reversed) to previous zone at: " + previousZoneTeleportTarget.position);
                    }
                    else
                    {
                        Debug.LogWarning("Previous zone teleport target not set on " + gameObject.name);
                    }
                }
                else // Exiting right or top
                {
                    if (nextZoneTeleportTarget != null)
                    {
                        other.transform.position = nextZoneTeleportTarget.position;
                        Debug.Log("Player teleported (reversed) to next zone at: " + nextZoneTeleportTarget.position);
                    }
                    else
                    {
                        Debug.LogWarning("Next zone teleport target not set on " + gameObject.name);
                    }
                }
            }
        }
    }
}
