using UnityEngine;

public class Quit : MonoBehaviour
{
    // Called when the exit button is clicked
    public void OnExitButtonClicked()
    {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
