using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance { get; private set; }

    public event Action<int, int> FruitsChanged;

    public int CollectedCount { get; private set; }
    public int TotalCollectibleCount { get; private set; }
    public int NewlyClaimedFruitCount { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeCollectibles();
        NotifyFruitsChanged();
    }

    public void CollectFruit(string fruitId)
    {
        CollectedCount++;

        bool isNewPermanentFruit = TryClaimPermanentFruit(fruitId);

        if (isNewPermanentFruit)
        {
            NewlyClaimedFruitCount++;
        }

        NotifyFruitsChanged();

        Debug.Log("Run Fruits: " + CollectedCount + " / " + TotalCollectibleCount);

        if (isNewPermanentFruit && SaveManager.Instance != null)
        {
            Debug.Log("Permanent Fruit Balance: " + SaveManager.Instance.Data.fruitBalance);
        }
    }

    private void InitializeCollectibles()
    {
        Collectible[] collectibles = FindObjectsByType<Collectible>(
            FindObjectsSortMode.None
        );

        TotalCollectibleCount = collectibles.Length;

        ValidateCollectibleIds(collectibles);
    }

    private bool TryClaimPermanentFruit(string fruitId)
    {
        if (string.IsNullOrWhiteSpace(fruitId))
        {
            Debug.LogWarning("Collectible has empty Fruit Id. Permanent fruit balance will not increase.");
            return false;
        }

        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager not found. Permanent fruit balance will not increase.");
            return false;
        }

        return SaveManager.Instance.TryClaimFruit(fruitId);
    }

    private void ValidateCollectibleIds(Collectible[] collectibles)
    {
        HashSet<string> usedIds = new HashSet<string>();

        foreach (Collectible collectible in collectibles)
        {
            if (collectible == null)
                continue;

            string fruitId = collectible.FruitId;

            if (string.IsNullOrWhiteSpace(fruitId))
            {
                Debug.LogWarning(
                    "Collectible has empty Fruit Id: " + collectible.name,
                    collectible
                );

                continue;
            }

            if (!usedIds.Add(fruitId))
            {
                Debug.LogWarning(
                    "Duplicate Fruit Id detected: " + fruitId,
                    collectible
                );
            }
        }
    }

    private void NotifyFruitsChanged()
    {
        FruitsChanged?.Invoke(CollectedCount, TotalCollectibleCount);
    }
}