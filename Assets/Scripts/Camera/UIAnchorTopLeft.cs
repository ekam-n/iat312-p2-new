using UnityEngine;

public class UIAnchorTopLeft : MonoBehaviour
{
    // Offset from the top-left anchor.
    public float xVec = 46.5f;
    public float yVec = -6f;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // Ensure that the RectTransform is anchored at the top left.
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);

        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition(); // Call this in Update if window resizing is allowed.
    }

    void UpdatePosition()
    {
        // Simply set the anchoredPosition relative to the top left.
        rectTransform.anchoredPosition = new Vector2(xVec, yVec);
    }
}
