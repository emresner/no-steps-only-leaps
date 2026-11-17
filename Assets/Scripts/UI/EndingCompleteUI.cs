using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingCompleteUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject completePanel;
    [SerializeField] private TMP_Text yourTimeText;
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text newFruitsText;
    [SerializeField] private Button backToMenuButton;
    [SerializeField] private SceneLoader sceneLoader;

    private void Awake()
    {
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.AddListener(BackToMainMenu);
        }

        Hide();
    }

    private void OnDestroy()
    {
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveListener(BackToMainMenu);
        }
    }

    public void Show(float finalTime)
    {
        bool isNewBestTime = TryUpdateBestTime(finalTime);

        if (yourTimeText != null)
        {
            yourTimeText.text = "YOUR TIME: " + GameTimer.FormatTime(finalTime);
        }

        if (bestTimeText != null)
        {
            bestTimeText.text = GetBestTimeText(isNewBestTime);
        }

        if (newFruitsText != null)
        {
            int newFruitCount = GetNewFruitCount();
            newFruitsText.text = "NEW FRUITS: +" + newFruitCount;
        }

        if (completePanel != null)
        {
            completePanel.SetActive(true);
        }
    }

    private bool TryUpdateBestTime(float finalTime)
    {
        if (SaveManager.Instance == null)
            return false;

        return SaveManager.Instance.TryUpdateBestTime(finalTime);
    }

    private string GetBestTimeText(bool isNewBestTime)
    {
        if (SaveManager.Instance == null)
            return "BEST TIME: --:--.--";

        SaveData saveData = SaveManager.Instance.Data;

        if (saveData == null || !saveData.hasBestTime)
            return "BEST TIME: --:--.--";

        string prefix = isNewBestTime ? "NEW BEST: " : "BEST TIME: ";

        return prefix + GameTimer.FormatTime(saveData.bestTime);
    }

    private int GetNewFruitCount()
    {
        if (CollectibleManager.Instance == null)
            return 0;

        return CollectibleManager.Instance.NewlyClaimedFruitCount;
    }

    private void Hide()
    {
        if (completePanel != null)
        {
            completePanel.SetActive(false);
        }
    }

    private void BackToMainMenu()
    {
        if (sceneLoader == null)
            return;

        sceneLoader.LoadMainMenu();
    }
}