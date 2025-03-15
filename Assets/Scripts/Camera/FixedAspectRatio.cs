using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspectRatio : MonoBehaviour
{
    // Set your target aspect ratio here. For example, 16:9.
    public float targetAspect = 16f / 9f;

    void Start()
    {
        Camera cam = GetComponent<Camera>();

        // Determine the game window's current aspect ratio.
        float windowAspect = (float)Screen.width / (float)Screen.height;
        // Calculate scale height relative to the target aspect ratio.
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            // The current window is taller than our target aspect ratio:
            // add letterboxing (black bars on top and bottom).
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else
        {
            // The current window is wider than our target aspect ratio:
            // add pillarboxing (black bars on the sides).
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
