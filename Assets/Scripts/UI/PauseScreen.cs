using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : UIScreen
{

    private void OnEnable()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void OnContinuePressed()
    {
        UIScreenManager.Instance.ShowScreen<GamePlayScreen>();
    }

    public void OnQuitPressed()
    {
        UIScreenManager.Instance.ShowScreen<MainMenuScreen>();
        SceneManager.LoadScene("MainMenuScene");
    }

}
