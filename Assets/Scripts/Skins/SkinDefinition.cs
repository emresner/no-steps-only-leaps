using System;
using UnityEngine;

[Serializable]
public class SkinDefinition
{
    [Header("Identity")]
    public string skinId;
    public string displayName;

    [Header("Unlock")]
    public int cost;
    public bool unlockedByDefault;

    [Header("Animator Controllers")]
    public RuntimeAnimatorController previewAnimatorController;
    public RuntimeAnimatorController gameplayAnimatorController;
}