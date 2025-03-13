using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // The target the camera will follow (e.g., the player's transform).
    public Transform target;
    
    // Offset from the target's position (set Z to -10 so the camera is in front of the scene).
    public Vector3 offset = new Vector3(0, 0, -10f);
    
    // Smoothing speed for camera movement.
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null)
            return;
        
        // Calculate the desired camera position.
        Vector3 desiredPosition = target.position + offset;
        // Smoothly interpolate between the current and desired position.
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
