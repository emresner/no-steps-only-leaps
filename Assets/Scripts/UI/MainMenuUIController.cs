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

        bestTimeText.text = "BEST: " + FormatTime(saveData.bestTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }
}