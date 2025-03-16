using UnityEngine;
using UnityEngine.UI;  // Ensure this is imported for working with UI Button

public class RespawnController : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    public Button respawnButton;      // Reference to the respawn button

    void Start()
    {
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        }
        else
        {
            Debug.LogError("Respawn Button is not assigned!");
        }
    }

    // This method is called when the respawn button is clicked
    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn(); // Call the Respawn method from the PlayerHealth script
        }
        else
        {
            Debug.LogError("PlayerHealth reference is missing!");
        }
    }
}
