using System.Collections;
using UnityEngine;

public class GameStartController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerSpawnController playerSpawnController;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private IntroCameraController introCameraController;
    [SerializeField] private StartFlagAnimationController startFlagAnimationController;

    private bool shouldPlayIntro;

    private void Awake()
    {
        shouldPlayIntro = GameLaunchContext.ShouldPlayStartIntro;

        if (introCameraController == null)
            return;

        if (shouldPlayIntro)
        {
            introCameraController.PrepareIntroCamera();
        }
        else
        {
            introCameraController.HideIntroCamera();
        }
    }

    private IEnumerator Start()
    {
        GameLaunchContext.ConsumeStartIntroFlag();

        PrepareTimer();
        PreparePlayer();

        if (shouldPlayIntro && introCameraController != null)
        {
            yield return introCameraController.PlayIntro();
        }

        yield return PlayInitialSpawnAndWait();

        StartTimer();
    }

    private void PrepareTimer()
    {
        if (gameTimer == null)
        {
            gameTimer = GameTimer.Instance;
        }

        if (gameTimer == null)
            return;

        gameTimer.ResetTimer();
        gameTimer.StopTimer();
    }

    private void PreparePlayer()
    {
        if (player == null)
            return;

        player.SetControlEnabled(false);
        player.SetVisualVisible(false);
        player.ResetGravityScale();
        player.Rigidbody.linearVelocity = Vector2.zero;
    }

    private IEnumerator PlayInitialSpawnAndWait()
    {
        if (playerSpawnController == null)
            yield break;

        bool spawnCompleted = false;

        void OnSpawnCompleted()
        {
            spawnCompleted = true;
        }

        playerSpawnController.SpawnCompleted += OnSpawnCompleted;

        if (startFlagAnimationController != null)
        {
            startFlagAnimationController.PlayStartAnimation();
        }

        playerSpawnController.PlayInitialSpawnSequence();

        while (!spawnCompleted)
        {
            yield return null;
        }

        playerSpawnController.SpawnCompleted -= OnSpawnCompleted;
    }

    private void StartTimer()
    {
        if (gameTimer == null)
        {
            gameTimer = GameTimer.Instance;
        }

        if (gameTimer == null)
            return;

        gameTimer.StartTimer();
    }
}