using UnityEngine;

public class UIAnchorTopLeft : MonoBehaviour
{
    public Camera targetCamera; // Assign the main camera
    private RectTransform rectTransform;
    public float xVec = 46.5f;
    public float yVec = -6f;
    public float zVec = 0f;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition(); // Call this in Update if window resizing is allowed
    }

    void UpdatePosition()
    {
        if (targetCamera == null) return;

        // Get the actual viewport area of the camera
        Rect camRect = targetCamera.pixelRect;

        // Convert pixel coordinates to UI canvas coordinates
        Vector2 screenPoint = new Vector2(camRect.xMin, camRect.yMax); // Top-left corner
        Vector3 worldPoint;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform.parent as RectTransform, screenPoint, targetCamera, out worldPoint);

        // Apply the new position
        Vector3 offset = new Vector3(xVec, yVec, zVec); // Adjust X (right) and Y (down)
        rectTransform.position = worldPoint + offset;
    }
}
