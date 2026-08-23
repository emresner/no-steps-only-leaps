using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform npcTransform;

    private bool hasTriggered;

    private void Awake()
    {
        if (npcTransform == null)
        {
            npcTransform = transform.parent;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered)
            return;

        if (!other.TryGetComponent(out Player player))
            return;

        hasTriggered = true;

        if (EndingManager.Instance != null)
        {
            EndingManager.Instance.StartEnding(player, npcTransform);
        }
    }
}