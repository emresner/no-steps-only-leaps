using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private const string WebGLSaveKey = "no_steps_only_leaps_save_json";

    public SaveData Data { get; private set; }

    private string SaveFilePath => Path.Combine(
        Application.persistentDataPath,
        "no_steps_only_leaps_save.json"
    );

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        Load();
    }

    public void Load()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadFromPlayerPrefs();
#else
        LoadFromFile();
#endif
    }

    public void Save()
    {
        EnsureDataExists();

        string json = JsonUtility.ToJson(Data, true);

#if UNITY_WEBGL && !UNITY_EDITOR
        PlayerPrefs.SetString(WebGLSaveKey, json);
        PlayerPrefs.Save();
#else
        File.WriteAllText(SaveFilePath, json);
#endif
    }

    private void LoadFromFile()
    {
        if (!File.Exists(SaveFilePath))
        {
            Data = new SaveData();
            Save();
            return;
        }

        string json = File.ReadAllText(SaveFilePath);
        LoadFromJson(json);
    }

    private void LoadFromPlayerPrefs()
    {
        if (!PlayerPrefs.HasKey(WebGLSaveKey))
        {
            Data = new SaveData();
            Save();
            return;
        }

        string json = PlayerPrefs.GetString(WebGLSaveKey);
        LoadFromJson(json);
    }

    private void LoadFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            Data = new SaveData();
            EnsureDataListsExist();
            return;
        }

        try
        {
            Data = JsonUtility.FromJson<SaveData>(json);
        }
        catch (System.ArgumentException)
        {
            Debug.LogWarning("Save data could not be read, starting with a fresh save.");
            Data = null;
        }

        if (Data == null)
        {
            Data = new SaveData();
        }

        EnsureDataListsExist();
        SanitizeData();
    }

    private void SanitizeData()
    {
        Data.fruitBalance = Mathf.Max(0, Data.fruitBalance);

        if (float.IsNaN(Data.bestTime) || float.IsInfinity(Data.bestTime) || Data.bestTime <= 0f)
        {
            Data.hasBestTime = false;
            Data.bestTime = 0f;
        }

        Data.claimedFruitIds.RemoveAll(string.IsNullOrWhiteSpace);
        Data.unlockedSkinIds.RemoveAll(string.IsNullOrWhiteSpace);
    }

    public bool HasClaimedFruit(string fruitId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(fruitId))
            return false;

        return Data.claimedFruitIds.Contains(fruitId);
    }

    public bool TryClaimFruit(string fruitId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(fruitId))
            return false;

        if (HasClaimedFruit(fruitId))
            return false;

        Data.claimedFruitIds.Add(fruitId);
        Data.fruitBalance++;

        Save();

        return true;
    }

    public bool TryUpdateBestTime(float finalTime)
    {
        EnsureDataExists();

        if (finalTime <= 0f)
            return false;

        if (!Data.hasBestTime || finalTime < Data.bestTime)
        {
            Data.hasBestTime = true;
            Data.bestTime = finalTime;

            Save();

            return true;
        }

        return false;
    }

    public bool IsSkinUnlocked(string skinId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(skinId))
            return false;

        return Data.unlockedSkinIds.Contains(skinId);
    }

    public bool CanAfford(int cost)
    {
        EnsureDataExists();

        return Data.fruitBalance >= cost;
    }

    public void EnsureSkinUnlocked(string skinId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(skinId))
            return;

        if (!Data.unlockedSkinIds.Contains(skinId))
        {
            Data.unlockedSkinIds.Add(skinId);
        }

        if (string.IsNullOrWhiteSpace(Data.selectedSkinId))
        {
            Data.selectedSkinId = skinId;
        }

        Save();
    }

    public bool TryUnlockSkin(string skinId, int cost)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(skinId))
            return false;

        if (IsSkinUnlocked(skinId))
            return false;

        if (!CanAfford(cost))
            return false;

        Data.fruitBalance -= cost;
        Data.unlockedSkinIds.Add(skinId);

        if (string.IsNullOrWhiteSpace(Data.selectedSkinId))
        {
            Data.selectedSkinId = skinId;
        }

        Save();

        return true;
    }

    public bool SelectSkin(string skinId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(skinId))
            return false;

        if (!IsSkinUnlocked(skinId))
            return false;

        Data.selectedSkinId = skinId;

        Save();

        return true;
    }

    public string GetSelectedSkinId()
    {
        EnsureDataExists();

        return Data.selectedSkinId;
    }

    public void EnsureSelectedSkin(string fallbackSkinId)
    {
        EnsureDataExists();

        if (string.IsNullOrWhiteSpace(fallbackSkinId))
            return;

        if (!IsSkinUnlocked(fallbackSkinId))
        {
            Data.unlockedSkinIds.Add(fallbackSkinId);
        }

        if (string.IsNullOrWhiteSpace(Data.selectedSkinId))
        {
            Data.selectedSkinId = fallbackSkinId;
            Save();
            return;
        }

        if (!IsSkinUnlocked(Data.selectedSkinId))
        {
            Data.selectedSkinId = fallbackSkinId;
            Save();
        }
    }

    private void EnsureDataExists()
    {
        if (Data == null)
        {
            Load();
        }

        if (Data == null)
        {
            Data = new SaveData();
        }

        EnsureDataListsExist();
    }

    private void EnsureDataListsExist()
    {
        if (Data.claimedFruitIds == null)
        {
            Data.claimedFruitIds = new System.Collections.Generic.List<string>();
        }

        if (Data.unlockedSkinIds == null)
        {
            Data.unlockedSkinIds = new System.Collections.Generic.List<string>();
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Debug/Delete Full Save")]
    private void DebugDeleteFullSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
        }

        Data = new SaveData();
        Save();

        Debug.Log("Full save reset.");
        Debug.Log("Save Path: " + SaveFilePath);
    }

    [ContextMenu("Debug/Reset Fruit Progress")]
    private void DebugResetFruitProgress()
    {
        EnsureDataExists();

        Data.fruitBalance = 0;
        Data.claimedFruitIds.Clear();

        Save();

        Debug.Log("Fruit progress reset.");
        Debug.Log("Fruit Balance: " + Data.fruitBalance);
    }

    [ContextMenu("Debug/Reset Best Time")]
    private void DebugResetBestTime()
    {
        EnsureDataExists();

        Data.hasBestTime = false;
        Data.bestTime = 0f;

        Save();

        Debug.Log("Best time reset.");
    }

    [ContextMenu("Debug/Reset Skin Progress")]
    private void DebugResetSkinProgress()
    {
        EnsureDataExists();

        Data.unlockedSkinIds.Clear();
        Data.selectedSkinId = string.Empty;

        Save();

        Debug.Log("Skin progress reset.");
    }

    [ContextMenu("Debug/Add 10 Fruits")]
    private void DebugAdd10Fruits()
    {
        EnsureDataExists();

        Data.fruitBalance += 10;

        Save();

        Debug.Log("Added 10 fruits.");
        Debug.Log("Fruit Balance: " + Data.fruitBalance);
    }

    [ContextMenu("Debug/Print Save Path")]
    private void DebugPrintSavePath()
    {
        Debug.Log("Save Path: " + SaveFilePath);
    }
#endif
}