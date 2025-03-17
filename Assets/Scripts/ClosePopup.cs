using UnityEngine;

public class ClosePopup : MonoBehaviour
{
    public GameObject popupImage; // Reference to the popup image UI element

    public void OnCloseButtonClicked()
    {
        if (popupImage != null)
        {
            popupImage.SetActive(false); // Hide the popup image when the button is clicked
        }
    }
}