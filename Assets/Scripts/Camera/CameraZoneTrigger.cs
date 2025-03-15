using UnityEngine;
using Unity.Cinemachine;

public class CameraZoneTrigger : MonoBehaviour
{
    // Reference to the virtual camera for the next zone.
    public CinemachineCamera nextZoneCamera;
    // Reference to the virtual camera for the previous zone.
    public CinemachineCamera previousZoneCamera;

    // Desired priorities for when the cameras should be active.
    public int activePriority = 10;
    public int inactivePriority = 0;

    [Tooltip("If true, reverses the trigger behavior. Exiting left will activate the previous zone camera and exiting right will activate the next zone camera.")]
    public bool reverseBehavior = false;

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        // Determine which side the player exited relative to this trigger's x position.
        if (other.transform.position.x < transform.position.x)
        {
            // Player exited to the left.
            if (!reverseBehavior)
            {
                // Normal behavior: activate next zone camera.
                if (nextZoneCamera != null)
                {
                    nextZoneCamera.Priority = activePriority;
                    if (previousZoneCamera != null)
                        previousZoneCamera.Priority = inactivePriority;
                    Debug.Log("Player exited left. Next zone camera activated.");
                }
            }
            else
            {
                // Reversed behavior: activate previous zone camera.
                if (previousZoneCamera != null)
                {
                    previousZoneCamera.Priority = activePriority;
                    if (nextZoneCamera != null)
                        nextZoneCamera.Priority = inactivePriority;
                    Debug.Log("Player exited left. (Reversed) Previous zone camera activated.");
                }
            }
        }
        else if (other.transform.position.x > transform.position.x)
        {
            // Player exited to the right.
            if (!reverseBehavior)
            {
                // Normal behavior: activate previous zone camera.
                if (previousZoneCamera != null)
                {
                    previousZoneCamera.Priority = activePriority;
                    if (nextZoneCamera != null)
                        nextZoneCamera.Priority = inactivePriority;
                    Debug.Log("Player exited right. Previous zone camera activated.");
                }
            }
            else
            {
                // Reversed behavior: activate next zone camera.
                if (nextZoneCamera != null)
                {
                    nextZoneCamera.Priority = activePriority;
                    if (previousZoneCamera != null)
                        previousZoneCamera.Priority = inactivePriority;
                    Debug.Log("Player exited right. (Reversed) Next zone camera activated.");
                }
            }
        }
    }
}
