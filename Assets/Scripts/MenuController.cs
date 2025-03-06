using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    

     public void StartGame()
    {
        // Show the instructions panel before starting the game
        instructionsPanel.SetActive(true);
    }

    public void CloseInstructions()
    {
        // Close the instructions panel and load the game scene
        instructionsPanel.SetActive(false);
        SceneManager.LoadScene("GameScene"); // Replace with your actual scene name
    }


    void Start()
    {
        DontDestroyOnLoad(gameObject); // Keeps MenuController & UI across scenes
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameObject instructionsPanel; // Reference to the panel

    public void OpenInstructions()
    {
        instructionsPanel.SetActive(true); // Show the panel
    }

   
}
