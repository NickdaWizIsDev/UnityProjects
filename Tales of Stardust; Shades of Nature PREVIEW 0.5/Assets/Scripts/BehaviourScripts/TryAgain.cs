using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TryAgain : MonoBehaviour
{
    public Button startButton;

    private void Start()
    {
        startButton = GetComponentInChildren<Button>();
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("2_MainMenu");
    }
}
