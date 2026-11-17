using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class TouchControlsController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject controlsRoot;
    [SerializeField] private GameObject jumpButton;

    [Header("Settings")]
    [SerializeField, Range(0f, 1f)] private float desktopJumpAlpha = 0.6f;

    [Header("Debug")]
    [SerializeField] private bool alwaysShow;

    private OnScreenButton jumpOnScreenButton;
    private Graphic jumpHitGraphic;
    private CanvasGroup jumpCanvasGroup;

    private void Awake()
    {
        CacheJumpComponents();
        SetControlsVisible(false);
        SetJumpVisible(false);
    }

    private void Update()
    {
        bool inGameplay = IsInGameplay();
        bool touchMode = alwaysShow || TouchModeDetector.IsTouchMode;

        SetControlsVisible(inGameplay && touchMode);
        SetJumpVisible(inGameplay);
        SetJumpInteractive(touchMode);
    }

    private void OnDisable()
    {
        SetControlsVisible(false);
        SetJumpVisible(false);
    }

    private bool IsInGameplay()
    {
        if (PauseManager.Instance != null && PauseManager.Instance.IsPaused)
            return false;

        if (GameTimer.Instance == null || !GameTimer.Instance.IsRunning)
            return false;

        return true;
    }

    private void CacheJumpComponents()
    {
        if (jumpButton == null)
            return;

        jumpOnScreenButton = jumpButton.GetComponent<OnScreenButton>();
        jumpHitGraphic = jumpButton.GetComponent<Graphic>();
        jumpCanvasGroup = jumpButton.GetComponent<CanvasGroup>();
    }

    private void SetControlsVisible(bool visible)
    {
        if (controlsRoot == null)
            return;

        if (controlsRoot.activeSelf == visible)
            return;

        controlsRoot.SetActive(visible);
    }

    private void SetJumpVisible(bool visible)
    {
        if (jumpButton == null)
            return;

        if (jumpButton.activeSelf == visible)
            return;

        jumpButton.SetActive(visible);
    }

    private void SetJumpInteractive(bool interactive)
    {
        if (jumpOnScreenButton != null && jumpOnScreenButton.enabled != interactive)
        {
            jumpOnScreenButton.enabled = interactive;
        }

        if (jumpHitGraphic != null && jumpHitGraphic.raycastTarget != interactive)
        {
            jumpHitGraphic.raycastTarget = interactive;
        }

        if (jumpCanvasGroup != null)
        {
            jumpCanvasGroup.alpha = interactive ? 1f : desktopJumpAlpha;
        }
    }
}
