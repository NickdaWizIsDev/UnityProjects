using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button tutoriel;
    public Button caveButton;
    public Button golemButton;

    private void Start()
    {
        tutoriel.onClick.AddListener(Level0);

        caveButton.onClick.AddListener(Level1);

        golemButton.onClick.AddListener(Level2);

        Time.timeScale = 1f;
    }

    public void Level0()
    {
        SceneManager.LoadScene("3_Tutorial");
        Time.timeScale = 1f;
    }

    public void Level1()
    {
        SceneManager.LoadScene("4_CaveSystem");
        Time.timeScale = 1f;
    }

    public void Level2()
    {
        SceneManager.LoadScene("Cutscene");
        Time.timeScale = 1f;
    }
}
