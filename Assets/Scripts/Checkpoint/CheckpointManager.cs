using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerSpawnController playerSpawnController;

    [Header("Default Spawn")]
    [SerializeField] private Transform defaultSpawnPoint;

    [Header("Return Settings")]
    [SerializeField] private float returnCooldown = 0.75f;

    private Checkpoint activeCheckpoint;
    private float lastReturnTime = -Mathf.Infinity;

    private Vector3 CurrentSpawnPosition
    {
        get
        {
            if (activeCheckpoint != null)
                return activeCheckpoint.SpawnPosition;

            if (defaultSpawnPoint != null)
                return defaultSpawnPoint.position;

            return playerSpawnController.transform.position;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (inputManager.ReturnToCheckpointPressed)
        {
            TryReturnToCheckpoint();
        }
    }

    public void SetActiveCheckpoint(Checkpoint checkpoint)
    {
        if (checkpoint == null)
            return;

        activeCheckpoint = checkpoint;
        activeCheckpoint.Activate();
    }

    private void TryReturnToCheckpoint()
    {
        if (playerSpawnController.IsSpawning)
            return;

        if (Time.time < lastReturnTime + returnCooldown)
            return;

        ReturnToCheckpoint();
    }

    private void ReturnToCheckpoint()
    {
        lastReturnTime = Time.time;
        playerSpawnController.PlaySpawnSequence(CurrentSpawnPosition);
    }
}