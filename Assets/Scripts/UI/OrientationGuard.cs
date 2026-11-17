using UnityEngine;

public class OrientationGuard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject overlay;

    private bool isShowing;

    private void Awake()
    {
        SetOverlayVisible(false);
    }

    private void Update()
    {
        bool isPortrait = Screen.height > Screen.width;

        if (isPortrait)
        {
            Time.timeScale = 0f;
        }

        if (isPortrait == isShowing)
            return;

        isShowing = isPortrait;
        SetOverlayVisible(isShowing);

        if (!isShowing)
        {
            RestoreTimeScale();
        }
    }

    private void OnDisable()
    {
        if (!isShowing)
            return;

        isShowing = false;
        RestoreTimeScale();
    }

    private void SetOverlayVisible(bool visible)
    {
        if (overlay == null)
            return;

        overlay.SetActive(visible);
    }

    private void RestoreTimeScale()
    {
        bool isGamePaused = PauseManager.Instance != null && PauseManager.Instance.IsPaused;

        Time.timeScale = isGamePaused ? 0f : 1f;
    }
}
