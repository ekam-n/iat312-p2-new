using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera will follow (e.g., the player's transform)
    public Transform target;
    
    // Offset from the target's position (set Z to -10 to keep the camera behind the scene)
    public Vector3 offset = new Vector3(0, 0, -10f);
    
    // Smoothing speed (set to 1 for instant follow, lower for slower follow)
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
            return;

        // Compute the desired position based on target position plus the offset.
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between the camera's current position and the desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
