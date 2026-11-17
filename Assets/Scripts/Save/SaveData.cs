using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int version = 1;

    public bool hasBestTime;
    public float bestTime;

    public int fruitBalance;

    public List<string> claimedFruitIds = new List<string>();

    public List<string> unlockedSkinIds = new List<string>();
    public string selectedSkinId;
}