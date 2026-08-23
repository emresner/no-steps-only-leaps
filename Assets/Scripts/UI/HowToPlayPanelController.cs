using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private Button openButton;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (openButton != null)
            openButton.onClick.AddListener(OpenPanel);

        if (closeButton != null)
            closeButton.onClick.AddListener(ClosePanel);

        ClosePanel();
    }

    private void OnDestroy()
    {
        if (openButton != null)
            openButton.onClick.RemoveListener(OpenPanel);

        if (closeButton != null)
            closeButton.onClick.RemoveListener(ClosePanel);
    }

    private void OpenPanel()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(true);
    }

    private void ClosePanel()
    {
        if (howToPlayPanel != null)
            howToPlayPanel.SetActive(false);
    }
}