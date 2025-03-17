using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    public GameObject popupImage; // Reference to the popup image UI element

    private bool hasPopupBeenShown = false; // Track if the popup has already been shown

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the player enters the trigger area and the popup hasn't been shown yet
        if (other.CompareTag("Player") && !hasPopupBeenShown)
        {
            ShowPopup(); // Show the popup when the player enters the trigger
            hasPopupBeenShown = true; // Mark the popup as shown
        }
    }

    private void ShowPopup()
    {
        if (popupImage != null)
        {
            popupImage.SetActive(true); // Show the popup image
        }
    }
}