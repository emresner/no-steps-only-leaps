using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static readonly int IsActivatedHash = Animator.StringToHash("isActivated");

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spawnPoint;

    public Vector3 SpawnPosition
    {
        get
        {
            if (spawnPoint != null)
                return spawnPoint.position;

            return transform.position;
        }
    }

    public bool IsActivated { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out Player player))
            return;

        CheckpointManager.Instance.SetActiveCheckpoint(this);
    }

    public void Activate()
    {
        if (IsActivated)
            return;

        IsActivated = true;
        animator.SetBool(IsActivatedHash, true);

        AudioManager.Instance?.PlayCheckpointActivate();
    }
}