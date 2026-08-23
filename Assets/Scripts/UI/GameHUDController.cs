using TMPro;
using UnityEngine;

public class GameHUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text fruitsText;

    private CollectibleManager collectibleManager;

    private void Start()
    {
        collectibleManager = CollectibleManager.Instance;

        if (collectibleManager != null)
        {
            collectibleManager.FruitsChanged += OnFruitsChanged;

            UpdateFruitsText(
                collectibleManager.CollectedCount,
                collectibleManager.TotalCollectibleCount
            );
        }
        else
        {
            UpdateFruitsText(0, 0);
        }
    }

    private void Update()
    {
        UpdateTimerText();
    }

    private void OnDestroy()
    {
        if (collectibleManager != null)
        {
            collectibleManager.FruitsChanged -= OnFruitsChanged;
        }
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        if (GameTimer.Instance == null)
            return;

        timerText.text = FormatTime(GameTimer.Instance.ElapsedTime);
    }

    private void OnFruitsChanged(int collectedCount, int totalCount)
    {
        UpdateFruitsText(collectedCount, totalCount);
    }

    private void UpdateFruitsText(int collectedCount, int totalCount)
    {
        if (fruitsText == null)
            return;

        fruitsText.text = $"{collectedCount} / {totalCount}";
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}