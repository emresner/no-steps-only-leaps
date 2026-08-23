using UnityEngine;

public class MarketSkinPreviewController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator previewAnimator;

    public void ShowSkin(SkinDefinition skin)
    {
        if (skin == null)
            return;

        if (previewAnimator == null)
            return;

        if (skin.previewAnimatorController == null)
        {
            Debug.LogWarning("Preview Animator Controller missing for skin: " + skin.skinId);
            return;
        }

        previewAnimator.runtimeAnimatorController = skin.previewAnimatorController;
    }
}