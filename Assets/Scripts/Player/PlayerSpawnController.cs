using System;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSpawnController : MonoBehaviour
{
    public event Action SpawnCompleted;

    [Header("References")]
    [SerializeField] private Player player;

    [Header("Spawn VFX")]
    [SerializeField] private SpawnVFX spawnVfxPrefab;
    [SerializeField] private Vector3 spawnVfxOffset = new Vector3(0f, 0.5f, 0f);

    [Header("Initial Spawn")]
    [SerializeField] private Transform initialSpawnPoint;

    public bool IsSpawning { get; private set; }

    private SpawnVFX currentSpawnVfx;

    public void PlayInitialSpawnSequence()
    {
        Vector3 spawnPosition = initialSpawnPoint != null
            ? initialSpawnPoint.position
            : player.transform.position;

        PlaySpawnSequence(spawnPosition);
    }

    public void PlaySpawnSequence(Vector3 spawnPosition)
    {
        IsSpawning = true;

        if (currentSpawnVfx != null)
        {
            currentSpawnVfx.Finished -= OnSpawnVfxFinished;
            Destroy(currentSpawnVfx.gameObject);
            currentSpawnVfx = null;
        }

        player.ResetStateForRespawn();
        player.SetControlEnabled(false);
        player.SetVisualVisible(false);

        Vector3 previousPosition = player.transform.position;

        player.transform.position = spawnPosition;
        player.Rigidbody.linearVelocity = Vector2.zero;

        Vector3 positionDelta = player.transform.position - previousPosition;

        CinemachineCore.OnTargetObjectWarped(
            player.transform,
            positionDelta
        );

        if (spawnVfxPrefab == null)
        {
            CompleteSpawnSequence(null);
            return;
        }

        currentSpawnVfx = Instantiate(
            spawnVfxPrefab,
            spawnPosition + spawnVfxOffset,
            Quaternion.identity
        );

        currentSpawnVfx.Finished += OnSpawnVfxFinished;
    }

    private void OnSpawnVfxFinished(SpawnVFX spawnVfx)
    {
        CompleteSpawnSequence(spawnVfx);
    }

    private void CompleteSpawnSequence(SpawnVFX spawnVfx)
    {
        if (spawnVfx != null)
        {
            spawnVfx.Finished -= OnSpawnVfxFinished;
        }

        player.SetVisualVisible(true);
        player.SetControlEnabled(true);

        if (spawnVfx != null)
        {
            Destroy(spawnVfx.gameObject);
        }

        if (currentSpawnVfx == spawnVfx)
        {
            currentSpawnVfx = null;
        }

        IsSpawning = false;

        SpawnCompleted?.Invoke();
    }
}