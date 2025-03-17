using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject instructionsPanel;
    public TextMeshProUGUI instructionText;

    // New variables for comic panel functionality
    public GameObject[] comicPanels;  // Array to hold comic panel GameObjects
    public Sprite[] comicSprites;     // Array to hold comic sprites (images as sprites)
    public Button nextButton;         // Reference to the Next button
    private int currentComicIndex = 0; // Track the current comic panel

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        // Ensure the nextButton has been assigned
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(OnNextButtonClicked); // Add listener to the Next button
        }

        // Ensure comic panels are not null
        if (comicPanels.Length > 0)
        {
            instructionText = GameObject.Find("InstructionText")?.GetComponent<TextMeshProUGUI>();

            if (instructionText == null)
            {
                panel.SetActive(false);  // Start by hiding all comic panels
            }
        }
        else
        {
            Debug.LogError("Comic Panels are not assigned!");
        }
    }

    public void StartGame()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true); // Show the instructions panel before starting the game
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
        instructionsPanel.SetActive(false); // Hide the instructions panel
    }
    else
    {
        Debug.LogError("Instructions Panel is missing!");
    }

    ShowComicPanel(currentComicIndex); // Show the first comic panel

    // Ensure the Next button appears after closing instructions
    if (nextButton != null)
    {
        nextButton.gameObject.SetActive(true);
    }
    else
    {
        Debug.LogError("Next Button is missing or not assigned!");
    }
}


    public void ShowComicPanel(int index)
    {
        // Hide all comic panels first
        foreach (var panel in comicPanels)
        {
            panel.SetActive(false);
        }

        // Show the current comic panel and set sprite
        if (comicPanels.Length > index)
        {
            GameObject currentPanel = comicPanels[index];
            currentPanel.SetActive(true); // Activate the panel

            Image panelImage = currentPanel.GetComponent<Image>();
            if (panelImage != null && comicSprites.Length > index)
            {
                // Set the sprite for the current comic
                panelImage.sprite = comicSprites[index];
            }
        }
    }

    public void OnNextButtonClicked()
{
    // Move to the next panel only if there are more panels left
    if (currentComicIndex  < comicPanels.Length)
    {
        ShowComicPanel(currentComicIndex); // Then show the correct panel
        currentComicIndex++;
    }
    else
    {
        nextButton.gameObject.SetActive(false); // Hide Next button when comics end
        
        // Hide all elements from the title scene before loading the game scene

        // Load the game scene after a delay for smooth transition
        Invoke("LoadGameScene", 0f); // Adjusted delay for smooth transition
        HideTitleSceneElements();

    }
}

private void HideTitleSceneElements()
{
    // Hide comic panels
    foreach (var panel in comicPanels)
    {
        panel.SetActive(false); // Hide all comic panels
    }

    // Hide instructions panel if it's active
    if (instructionsPanel != null)
    {
        instructionsPanel.SetActive(false);
    }

    // Hide the "Next" button
    if (nextButton != null)
    {
        nextButton.gameObject.SetActive(false);
    }
}

public void LoadGameScene()
{
    // Load the game scene
    SceneManager.LoadScene("GameScene"); // Replace "GameScene" with your actual game scene name
}


    public void OpenInstructions()
    {
        if (instructionsPanel != null)
        {
            instructionsPanel.SetActive(true); // Show the panel
        }

        if (instructionText != null)
        {
            instructionText.text = "Here are the game instructions!";
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
