using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuMarketController : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject marketPanel;

    [Header("Backgrounds")]
    [SerializeField] private GameObject menuBackground;
    [SerializeField] private GameObject marketBackground;

    [Header("Skin Data")]
    [SerializeField] private SkinDatabase skinDatabase;

    [Header("Market Texts")]
    [SerializeField] private TMP_Text marketFruitBalanceText;
    [SerializeField] private TMP_Text skinNameText;
    [SerializeField] private TMP_Text skinCostText;
    [SerializeField] private GameObject skinCostFruitIconGroup;

    [Header("Preview")]
    [SerializeField] private MarketSkinPreviewController skinPreviewController;

    [Header("Buttons")]
    [SerializeField] private Button marketButton;
    [SerializeField] private Button marketBackButton;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button unlockButton;
    [SerializeField] private Button selectButton;

    private TMP_Text unlockButtonText;
    private TMP_Text selectButtonText;

    private int currentSkinIndex;

    private SkinDefinition[] Skins
    {
        get
        {
            if (skinDatabase == null)
                return null;

            return skinDatabase.Skins;
        }
    }

    private SkinDefinition CurrentSkin
    {
        get
        {
            SkinDefinition[] availableSkins = Skins;

            if (availableSkins == null || availableSkins.Length == 0)
                return null;

            currentSkinIndex = Mathf.Clamp(currentSkinIndex, 0, availableSkins.Length - 1);
            return availableSkins[currentSkinIndex];
        }
    }

    private void Awake()
    {
        CacheButtonTexts();
        RegisterButtonEvents();

        CloseMarket();
    }

    private void Start()
    {
        EnsureDefaultSkinState();
        SetCurrentIndexToSelectedSkin();
        RefreshMarketUI();
    }

    private void OnDestroy()
    {
        UnregisterButtonEvents();
    }

    private void CacheButtonTexts()
    {
        if (unlockButton != null)
        {
            unlockButtonText = unlockButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (selectButton != null)
        {
            selectButtonText = selectButton.GetComponentInChildren<TMP_Text>(true);
        }
    }

    private void RegisterButtonEvents()
    {
        if (marketButton != null)
        {
            marketButton.onClick.AddListener(OpenMarket);
        }

        if (marketBackButton != null)
        {
            marketBackButton.onClick.AddListener(CloseMarket);
        }

        if (leftArrowButton != null)
        {
            leftArrowButton.onClick.AddListener(ShowPreviousSkin);
        }

        if (rightArrowButton != null)
        {
            rightArrowButton.onClick.AddListener(ShowNextSkin);
        }

        if (unlockButton != null)
        {
            unlockButton.onClick.AddListener(UnlockCurrentSkin);
        }

        if (selectButton != null)
        {
            selectButton.onClick.AddListener(SelectCurrentSkin);
        }
    }

    private void UnregisterButtonEvents()
    {
        if (marketButton != null)
        {
            marketButton.onClick.RemoveListener(OpenMarket);
        }

        if (marketBackButton != null)
        {
            marketBackButton.onClick.RemoveListener(CloseMarket);
        }

        if (leftArrowButton != null)
        {
            leftArrowButton.onClick.RemoveListener(ShowPreviousSkin);
        }

        if (rightArrowButton != null)
        {
            rightArrowButton.onClick.RemoveListener(ShowNextSkin);
        }

        if (unlockButton != null)
        {
            unlockButton.onClick.RemoveListener(UnlockCurrentSkin);
        }

        if (selectButton != null)
        {
            selectButton.onClick.RemoveListener(SelectCurrentSkin);
        }
    }

    private void OpenMarket()
    {
        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
        }

        if (marketPanel != null)
        {
            marketPanel.SetActive(true);
        }

        if (menuBackground != null)
        {
            menuBackground.SetActive(false);
        }

        if (marketBackground != null)
        {
            marketBackground.SetActive(true);
        }

        SetCurrentIndexToSelectedSkin();
        RefreshMarketUI();
    }

    private void CloseMarket()
    {
        if (marketPanel != null)
        {
            marketPanel.SetActive(false);
        }

        if (mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(true);
        }

        if (marketBackground != null)
        {
            marketBackground.SetActive(false);
        }

        if (menuBackground != null)
        {
            menuBackground.SetActive(true);
        }
    }

    private void ShowPreviousSkin()
    {
        SkinDefinition[] availableSkins = Skins;

        if (availableSkins == null || availableSkins.Length == 0)
            return;

        currentSkinIndex--;

        if (currentSkinIndex < 0)
        {
            currentSkinIndex = availableSkins.Length - 1;
        }

        RefreshMarketUI();
    }

    private void ShowNextSkin()
    {
        SkinDefinition[] availableSkins = Skins;

        if (availableSkins == null || availableSkins.Length == 0)
            return;

        currentSkinIndex++;

        if (currentSkinIndex >= availableSkins.Length)
        {
            currentSkinIndex = 0;
        }

        RefreshMarketUI();
    }

    private void UnlockCurrentSkin()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
            return;

        if (SaveManager.Instance == null)
            return;

        bool unlocked = SaveManager.Instance.TryUnlockSkin(
            skin.skinId,
            skin.cost
        );

        if (unlocked)
        {
            RefreshMarketUI();
        }
    }

    private void SelectCurrentSkin()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
            return;

        if (SaveManager.Instance == null)
            return;

        bool selected = SaveManager.Instance.SelectSkin(skin.skinId);

        if (selected)
        {
            RefreshMarketUI();
        }
    }

    private void RefreshMarketUI()
    {
        RefreshFruitBalanceText();
        RefreshSkinText();
        RefreshButtonStates();
        RefreshPreview();
    }

    private void RefreshPreview()
    {
        if (skinPreviewController == null)
            return;

        skinPreviewController.ShowSkin(CurrentSkin);
    }

    private void RefreshFruitBalanceText()
    {
        if (marketFruitBalanceText == null)
            return;

        if (SaveManager.Instance == null || SaveManager.Instance.Data == null)
        {
            marketFruitBalanceText.text = "0";
            return;
        }

        marketFruitBalanceText.text = SaveManager.Instance.Data.fruitBalance.ToString();
    }

    private void RefreshSkinText()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
        {
            if (skinNameText != null)
            {
                skinNameText.text = "NO SKIN";
            }

            if (skinCostText != null)
            {
                skinCostText.text = "";
            }

            SetSkinCostFruitIconVisible(false);

            return;
        }

        if (skinNameText != null)
        {
            skinNameText.text = skin.displayName;
        }

        if (skinCostText != null)
        {
            if (IsCurrentSkinSelected())
            {
                skinCostText.text = "SELECTED";
                SetSkinCostFruitIconVisible(false);
            }
            else if (IsCurrentSkinUnlocked())
            {
                skinCostText.text = "UNLOCKED";
                SetSkinCostFruitIconVisible(false);
            }
            else
            {
                skinCostText.text = "COST: " + skin.cost;
                SetSkinCostFruitIconVisible(true);
            }
        }
    }

    private void RefreshButtonStates()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
        {
            SetButtonVisible(unlockButton, false);
            SetButtonVisible(selectButton, false);
            return;
        }

        bool isUnlocked = IsCurrentSkinUnlocked();
        bool isSelected = IsCurrentSkinSelected();
        bool canAfford = CanAffordCurrentSkin();

        SetButtonVisible(unlockButton, !isUnlocked);
        SetButtonVisible(selectButton, isUnlocked);

        if (unlockButton != null)
        {
            unlockButton.interactable = !isUnlocked && canAfford;
        }

        if (unlockButtonText != null)
        {
            unlockButtonText.text = canAfford ? "UNLOCK" : "NOT ENOUGH";
        }

        if (selectButton != null)
        {
            selectButton.interactable = isUnlocked && !isSelected;
        }

        if (selectButtonText != null)
        {
            selectButtonText.text = isSelected ? "SELECTED" : "SELECT";
        }
    }

    private bool IsCurrentSkinUnlocked()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
            return false;

        if (skin.unlockedByDefault)
            return true;

        if (SaveManager.Instance == null)
            return false;

        return SaveManager.Instance.IsSkinUnlocked(skin.skinId);
    }

    private bool IsCurrentSkinSelected()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
            return false;

        if (SaveManager.Instance == null)
            return false;

        return SaveManager.Instance.GetSelectedSkinId() == skin.skinId;
    }

    private bool CanAffordCurrentSkin()
    {
        SkinDefinition skin = CurrentSkin;

        if (skin == null)
            return false;

        if (SaveManager.Instance == null)
            return false;

        return SaveManager.Instance.CanAfford(skin.cost);
    }

    private void EnsureDefaultSkinState()
    {
        if (SaveManager.Instance == null)
            return;

        SkinDefinition[] availableSkins = Skins;

        if (availableSkins == null || availableSkins.Length == 0)
            return;

        SkinDefinition fallbackSkin = skinDatabase.GetFallbackSkin();

        if (fallbackSkin == null)
            return;

        foreach (SkinDefinition skin in availableSkins)
        {
            if (skin == null)
                continue;

            if (skin.unlockedByDefault)
            {
                SaveManager.Instance.EnsureSkinUnlocked(skin.skinId);
            }
        }

        SaveManager.Instance.EnsureSelectedSkin(fallbackSkin.skinId);
    }

    private void SetCurrentIndexToSelectedSkin()
    {
        SkinDefinition[] availableSkins = Skins;

        if (availableSkins == null || availableSkins.Length == 0)
            return;

        if (SaveManager.Instance == null)
        {
            currentSkinIndex = 0;
            return;
        }

        string selectedSkinId = SaveManager.Instance.GetSelectedSkinId();

        for (int i = 0; i < availableSkins.Length; i++)
        {
            if (availableSkins[i] == null)
                continue;

            if (availableSkins[i].skinId == selectedSkinId)
            {
                currentSkinIndex = i;
                return;
            }
        }

        currentSkinIndex = 0;
    }

    private void SetButtonVisible(Button button, bool isVisible)
    {
        if (button == null)
            return;

        button.gameObject.SetActive(isVisible);
    }

    private void SetSkinCostFruitIconVisible(bool isVisible)
    {
        if (skinCostFruitIconGroup == null)
            return;

        skinCostFruitIconGroup.SetActive(isVisible);
    }
}