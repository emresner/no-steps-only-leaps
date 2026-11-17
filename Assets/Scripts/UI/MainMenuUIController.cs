using TMPro;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text bestTimeText;

    private void Start()
    {
        UpdateBestTimeText();
    }

    private void UpdateBestTimeText()
    {
        if (bestTimeText == null)
            return;

        if (SaveManager.Instance == null)
        {
            bestTimeText.text = "BEST: --:--.--";
            return;
        }

        SaveData saveData = SaveManager.Instance.Data;

        if (saveData == null || !saveData.hasBestTime)
        {
            bestTimeText.text = "BEST: --:--.--";
            return;
        }

        bestTimeText.text = "BEST: " + GameTimer.FormatTime(saveData.bestTime);
    }
}