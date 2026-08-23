using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonClickSoundBinder : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool includeInactiveButtons = true;

    [Header("Optional Exclusions")]
    [SerializeField] private Button[] excludedButtons;

    private readonly List<Button> boundButtons = new List<Button>();

    private void Awake()
    {
        BindButtons();
    }

    private void OnDestroy()
    {
        UnbindButtons();
    }

    private void BindButtons()
    {
        Button[] buttons = GetComponentsInChildren<Button>(includeInactiveButtons);

        foreach (Button button in buttons)
        {
            if (button == null)
                continue;

            if (IsExcluded(button))
                continue;

            button.onClick.AddListener(PlayButtonClick);
            boundButtons.Add(button);
        }
    }

    private void UnbindButtons()
    {
        foreach (Button button in boundButtons)
        {
            if (button == null)
                continue;

            button.onClick.RemoveListener(PlayButtonClick);
        }

        boundButtons.Clear();
    }

    private bool IsExcluded(Button button)
    {
        if (excludedButtons == null)
            return false;

        foreach (Button excludedButton in excludedButtons)
        {
            if (excludedButton == button)
                return true;
        }

        return false;
    }

    private void PlayButtonClick()
    {
        AudioManager.Instance?.PlayButtonClick();
    }
}