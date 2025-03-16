using UnityEngine;
using TMPro;  // Ensure you have this namespace for TextMeshPro
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Add this for UI Button functionality

public class MenuController : MonoBehaviour
{
    public GameObject instructionsPanel; // Reference to the instructions panel
    public TextMeshProUGUI instructionText; // Reference to the TextMeshProUGUI component
    public Button respawnButton; // Reference to the Respawn Button UI element
    public PlayerHealth playerHealth; // Reference to the PlayerHealth script

    void Start()
    {
        // Ensures the MenuController persists across scenes
        DontDestroyOnLoad(gameObject);

        // Check if instructionsPanel is not assigned in the Inspector
        if (instructionsPanel == null)
        {
            Debug.LogWarning("Instructions Panel is not assigned in the Inspector!");
        }

        // Check if instructionText is not assigned
        if (instructionText == null)
        {
            instructionText = GameObject.Find("InstructionText")?.GetComponent<TextMeshProUGUI>();

            if (instructionText == null)
            {
                Debug.LogError("Instruction TextMeshProUGUI is missing or destroyed!");
            }
        }

        // Ensure that the respawn button is only active if the player is dead
        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(false); // Start with the respawn button hidden
            respawnButton.onClick.AddListener(OnRespawnButtonClicked);
        }
        else
        {
            Debug.LogError("Respawn Button is not assigned!");
        }
    }

    public void StartGame()
    {
        if (instructionsPanel != null)
        {
            // Show the instructions panel before starting the game
            instructionsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Instructions Panel is missing!");
        }
    }

    public void CloseInstructions()
    {
        if (instructionsPanel != null)
        {
            // Close the instructions panel and load the game scene
            instructionsPanel.SetActive(false);
            SceneManager.LoadScene("GameScene"); // Replace with your actual scene name
        }
        else
        {
            Debug.LogError("Instructions Panel is missing!");
        }

        // Ensure instructionText is valid before using it
        if (instructionText != null)
        {
            // Example: Reset instructions text after closing the panel
            instructionText.text = "";
        }
        else
        {
            Debug.LogError("Instruction TextMeshProUGUI is missing!");
        }
    }

    public void OpenInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true); // Show the panel
        }
        else
        {
            Debug.LogError("Instructions Panel is missing!");
        }

        // Ensure instructionText is valid before using it
        if (instructionText != null)
        {
            // Example: Set instructions text when opening the panel
            instructionText.text = "Here are the game instructions!";
        }
        else
        {
            Debug.LogError("Instruction TextMeshProUGUI is missing!");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // New method to handle the respawn button click
    private void OnRespawnButtonClicked()
    {
        if (playerHealth != null)
        {
            playerHealth.Respawn(); // Call the Respawn method on PlayerHealth
            respawnButton.gameObject.SetActive(false); // Hide the respawn button again after clicking
        }
    }

    // Call this when the player dies to show the respawn button
    public void ShowRespawnButton()
    {
        if (respawnButton != null)
        {
            respawnButton.gameObject.SetActive(true); // Show the respawn button when the player is dead
        }
    }
}
