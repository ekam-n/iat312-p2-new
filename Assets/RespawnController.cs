using UnityEngine;
using UnityEngine.UI;  // Ensure this is included for button functionality

public class Respawner : MonoBehaviour
{
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script
    public Button respawnButton;      // Reference to the respawn button

    void Start()
    {
        if (respawnButton != null)
        {
            respawnButton.onClick.AddListener(OnRespawnButtonClicked); // Add listener to button
        }
        else
        {
            Debug.LogError("Respawn Button is not assigned!");
        }
    }

    // Rename the method to avoid conflict if necessary
    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();  // Call the Respawn method from PlayerHealth script
        }
        else
        {
            Debug.LogError("PlayerHealth reference is missing!");
        }
    }
}
