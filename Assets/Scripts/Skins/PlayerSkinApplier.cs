using UnityEngine;

public class PlayerSkinApplier : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SkinDatabase skinDatabase;

    private void Start()
    {
        ApplySelectedSkin();
    }

    public void ApplySelectedSkin()
    {
        if (playerAnimator == null)
            return;

        if (skinDatabase == null)
            return;

        SkinDefinition selectedSkin = GetSelectedSkin();

        if (selectedSkin == null)
            return;

        if (selectedSkin.gameplayAnimatorController == null)
        {
            Debug.LogWarning("Gameplay Animator Controller missing for skin: " + selectedSkin.skinId);
            return;
        }

        playerAnimator.runtimeAnimatorController = selectedSkin.gameplayAnimatorController;
    }

    private SkinDefinition GetSelectedSkin()
    {
        if (SaveManager.Instance != null)
        {
            string selectedSkinId = SaveManager.Instance.GetSelectedSkinId();

            if (!string.IsNullOrEmpty(selectedSkinId))
            {
                SkinDefinition savedSkin = skinDatabase.GetSkinById(selectedSkinId);

                if (savedSkin != null)
                    return savedSkin;
            }
        }

        return skinDatabase.GetFallbackSkin();
    }
}