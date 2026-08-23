using UnityEngine;

[CreateAssetMenu(
    fileName = "SkinDatabase",
    menuName = "No Steps Only Leaps/Skins/Skin Database"
)]
public class SkinDatabase : ScriptableObject
{
    [SerializeField] private SkinDefinition[] skins;

    public SkinDefinition[] Skins => skins;

    public SkinDefinition GetSkinById(string skinId)
    {
        if (skins == null)
            return null;

        foreach (SkinDefinition skin in skins)
        {
            if (skin == null)
                continue;

            if (skin.skinId == skinId)
                return skin;
        }

        return null;
    }

    public SkinDefinition GetFallbackSkin()
    {
        if (skins == null || skins.Length == 0)
            return null;

        foreach (SkinDefinition skin in skins)
        {
            if (skin == null)
                continue;

            if (skin.unlockedByDefault)
                return skin;
        }

        return skins[0];
    }
}