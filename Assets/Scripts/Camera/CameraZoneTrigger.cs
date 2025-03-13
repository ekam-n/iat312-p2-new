using UnityEngine;
using Unity.Cinemachine;

public class ZoneTransitionTrigger : MonoBehaviour
{
    // Reference to the virtual camera for the next zone.
    public CinemachineCamera nextZoneCamera;
    // Reference to the virtual camera for the previous zone.
    public CinemachineCamera previousZoneCamera;

    // Optionally, specify the desired priority values.
    public int nextZonePriority = 10;
    public int previousZonePriority = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger (ensure the player has the "Player" tag).
        if (other.CompareTag("Player"))
        {
            if(nextZoneCamera != null)
            {
                nextZoneCamera.Priority = nextZonePriority;
                Debug.Log("Next zone camera priority set to " + nextZonePriority);
            }
            if(previousZoneCamera != null)
            {
                previousZoneCamera.Priority = previousZonePriority;
                Debug.Log("Previous zone camera priority set to " + previousZonePriority);
            }
        }
    }
}
