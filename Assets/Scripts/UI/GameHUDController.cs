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

        timerText.text = GameTimer.FormatTime(GameTimer.Instance.ElapsedTime);
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
}