using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string MainMenuSceneName = "MainMenuScene";
    private const string GameSceneName = "GameScene";

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;

        GameLaunchContext.SkipStartIntro();

        SceneManager.LoadScene(MainMenuSceneName);
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;

        GameLaunchContext.RequestStartIntro();

        SceneManager.LoadScene(GameSceneName);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        GameLaunchContext.SkipStartIntro();

        SceneManager.LoadScene(GameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Quit Game requested.");
    }
}