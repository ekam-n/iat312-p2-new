using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject instructionsPanel; // Panel for text instructions
    public TextMeshProUGUI instructionText; // Text display
    public GameObject[] instructionImages; // PNG instruction screens
    public GameObject nextButton; // UI button to proceed
    private int currentIndex = 0;

    void Start()
{
    DontDestroyOnLoad(gameObject);

    if (instructionsPanel == null)
        Debug.LogWarning("Instructions Panel is not assigned in the Inspector!");

    if (instructionText == null)
    {
        instructionText = GameObject.Find("InstructionText")?.GetComponent<TextMeshProUGUI>();
        if (instructionText == null)
            Debug.LogError("Instruction TextMeshProUGUI is missing or destroyed!");
    }

    // Hide all PNG screens at the start
    foreach (var img in instructionImages)
    {
        img.SetActive(false);
    }

    // Make sure the "Next" button is hidden at the start
    if (nextButton != null)
        nextButton.SetActive(false);
}


    public void StartGame()
    {
        if (instructionsPanel != null)
        {
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
        instructionsPanel.SetActive(false);

        // Show first PNG instruction screen
        if (instructionImages.Length > 0)
        {
            instructionImages[0].SetActive(true);
            currentIndex = 0;

            // Show "Next" button only AFTER instructions are closed
            if (nextButton != null)
                nextButton.SetActive(true);
        }
        else
        {
            // If no images exist, go directly to the game
            SceneManager.LoadScene("GameScene");
        }
    }
    else
    {
        Debug.LogError("Instructions Panel is missing!");
    }

    if (instructionText != null)
        instructionText.text = "";
}


    public void NextScreen()
    {
        if (currentIndex < instructionImages.Length - 1)
        {
            instructionImages[currentIndex].SetActive(false);
            currentIndex++;
            instructionImages[currentIndex].SetActive(true);
        }
        else
        {
            // After last screen, load the game
            SceneManager.LoadScene("GameScene");
        }
    }

    public void OpenInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Instructions Panel is missing!");
        }

        if (instructionText != null)
            instructionText.text = "Here are the game instructions!";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
