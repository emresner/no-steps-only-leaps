using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private SceneLoader sceneLoader;

    public static PauseManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(BackToMainMenu);
        }

        HidePausePanel();
    }

    private void Update()
    {
        if (inputManager == null)
            return;

        if (!inputManager.PausePressed)
            return;

        TogglePause();
    }

    private void OnDestroy()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveListener(ResumeGame);
        }

        if (restartButton != null)
        {
            restartButton.onClick.RemoveListener(RestartGame);
        }

        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveListener(BackToMainMenu);
        }
    }

    private void TogglePause()
    {
        if (IsPaused)
        {
            ResumeGame();
            return;
        }

        PauseGame();
    }

    private void PauseGame()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    private void ResumeGame()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        HidePausePanel();
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;

        if (sceneLoader != null)
        {
            sceneLoader.RestartGame();
        }
    }

    private void BackToMainMenu()
    {
        Time.timeScale = 1f;

        if (sceneLoader != null)
        {
            sceneLoader.LoadMainMenu();
        }
    }

    private void HidePausePanel()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
}