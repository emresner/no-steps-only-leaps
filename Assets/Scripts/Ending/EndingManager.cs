using System.Collections;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public static EndingManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private EndingCameraController endingCameraController;
    [SerializeField] private EndingDialogueUI endingDialogueUI;
    [SerializeField] private EndingCompleteUI endingCompleteUI;
    [SerializeField] private Transform endingPlayerStandPoint;

    [Header("Player Ending Movement")]
    [SerializeField] private float playerWalkSpeed = 2.5f;
    [SerializeField] private float standPointArriveDistance = 0.05f;
    [SerializeField] private int playerFinalFacingDirection = 1;

    public bool EndingStarted { get; private set; }
    public float FinalTime { get; private set; }

    private Coroutine endingRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartEnding(Player player, Transform npcTransform)
    {
        if (EndingStarted)
            return;

        if (player == null)
            return;

        EndingStarted = true;

        StopGameTimer();

        endingRoutine = StartCoroutine(EndingSequence(player, npcTransform));
    }

    private IEnumerator EndingSequence(Player player, Transform npcTransform)
    {
        yield return WaitUntilPlayerLands(player);

        player.EnterEndingMode();

        if (endingPlayerStandPoint != null)
        {
            yield return MovePlayerToStandPoint(player);
        }

        player.SetEndingWalking(false);
        player.SetFacingDirection(playerFinalFacingDirection);
        player.Rigidbody.linearVelocity = Vector2.zero;

        if (endingCameraController != null)
        {
            endingCameraController.FocusOnEnding(player.transform, npcTransform);
        }

        if (endingDialogueUI != null)
        {
            endingDialogueUI.StartDialogue(OnDialogueCompleted);
        }

        Debug.Log("Ending started!");
        Debug.Log("Final Time: " + FinalTime);
    }

    private IEnumerator WaitUntilPlayerLands(Player player)
    {
        while (player != null && !player.IsGroundDetected())
        {
            yield return null;
        }
    }

    private IEnumerator MovePlayerToStandPoint(Player player)
    {
        float targetX = endingPlayerStandPoint.position.x;

        while (player != null && Mathf.Abs(player.transform.position.x - targetX) > standPointArriveDistance)
        {
            float direction = Mathf.Sign(targetX - player.transform.position.x);

            player.SetFacingDirection(direction > 0f ? 1 : -1);
            player.SetEndingWalking(true);

            player.Rigidbody.linearVelocity = new Vector2(
                direction * playerWalkSpeed,
                player.Rigidbody.linearVelocity.y
            );

            yield return null;
        }

        if (player == null)
            yield break;

        player.Rigidbody.linearVelocity = Vector2.zero;
        player.SetEndingWalking(false);

        Vector3 finalPosition = player.transform.position;
        finalPosition.x = targetX;
        player.transform.position = finalPosition;
    }

    private void StopGameTimer()
    {
        if (GameTimer.Instance == null)
            return;

        FinalTime = GameTimer.Instance.ElapsedTime;
        GameTimer.Instance.StopTimer();
    }

    private void OnDialogueCompleted()
    {
        if (endingCompleteUI != null)
        {
            endingCompleteUI.Show(FinalTime);
        }

        Debug.Log("Ending dialogue completed.");
    }
}