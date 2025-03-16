using UnityEngine;
using UnityEngine.UI;

public class RespawnButton : MonoBehaviour
{
    public Button respawnButton;
    public PlayerHealth playerHealth;  // Reference to the PlayerHealth script

    void Start()
    {
        // Set up the button listener to call the respawn function when clicked
        respawnButton.onClick.AddListener(OnRespawnButtonClicked);
    }

    void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn();  // Call the Respawn method on PlayerHealth
        }
    }
}
