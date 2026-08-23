using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string fruitId;

    [Header("VFX")]
    [SerializeField] private CollectibleVFX collectedVfxPrefab;
    [SerializeField] private Vector3 vfxOffset;

    private bool isCollected;

    public string FruitId => fruitId;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        Collect();
    }

    private void Collect()
    {
        isCollected = true;

        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.CollectFruit(fruitId);
        }

        AudioManager.Instance?.PlayCollectiblePickup();

        if (collectedVfxPrefab != null)
        {
            Instantiate(
                collectedVfxPrefab,
                transform.position + vfxOffset,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}