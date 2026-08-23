using UnityEngine;

public class MenuCharacterWalker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visualTransform;

    [Header("Movement")]
    [SerializeField] private float moveDistance = 1.2f;
    [SerializeField] private float moveSpeed = 1.2f;
    [SerializeField] private float waitAtEdgeDuration = 0.4f;

    private Vector3 startPosition;
    private Vector3 leftTarget;
    private Vector3 rightTarget;
    private Vector3 currentTarget;

    private float waitTimer;
    private int moveDirection = 1;

    private void Awake()
    {
        startPosition = transform.position;

        leftTarget = startPosition + Vector3.left * moveDistance;
        rightTarget = startPosition + Vector3.right * moveDistance;

        currentTarget = rightTarget;
    }

    private void Update()
    {
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        MoveCharacter();
    }

    private void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            currentTarget,
            moveSpeed * Time.deltaTime
        );

        FaceMovementDirection();

        float distanceToTarget = Vector3.Distance(transform.position, currentTarget);

        if (distanceToTarget > 0.02f)
            return;

        SwitchTarget();
    }

    private void SwitchTarget()
    {
        waitTimer = waitAtEdgeDuration;

        if (currentTarget == rightTarget)
        {
            currentTarget = leftTarget;
            moveDirection = -1;
        }
        else
        {
            currentTarget = rightTarget;
            moveDirection = 1;
        }

        FaceMovementDirection();
    }

    private void FaceMovementDirection()
    {
        if (visualTransform == null)
            return;

        Vector3 scale = visualTransform.localScale;
        scale.x = Mathf.Abs(scale.x) * moveDirection;
        visualTransform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 gizmoStart = Application.isPlaying ? startPosition : transform.position;
        Vector3 gizmoLeft = gizmoStart + Vector3.left * moveDistance;
        Vector3 gizmoRight = gizmoStart + Vector3.right * moveDistance;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gizmoLeft, gizmoRight);
        Gizmos.DrawSphere(gizmoLeft, 0.08f);
        Gizmos.DrawSphere(gizmoRight, 0.08f);
    }
}