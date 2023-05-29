using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas pauseCanvas;
    public GameObject continueButton;
    public GameObject mainMenuButton;

    private bool isPaused = false;

    private void Start()
    {
        // Initially hide the pause menu
        pauseCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Check for the Escape key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the pause state
            isPaused = !isPaused;

            // If the game is paused, show the pause menu
            if (isPaused)
            {
                Time.timeScale = 0f;  // Pause the game
                pauseCanvas.gameObject.SetActive(true);

                // Set the focus on the continue button for easy keyboard navigation
                continueButton.SetActive(true);
                continueButton.GetComponent<UnityEngine.UI.Button>().Select();
            }
            else
            {
                // Resume the game
                Time.timeScale = 1f;
                pauseCanvas.gameObject.SetActive(false);
            }
        }
    }

    public void Continue()
    {
        // Resume the game
        Time.timeScale = 1f;
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
    }

    public void MainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("2_MainMenu");
    }
}
