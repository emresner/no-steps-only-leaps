using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform[] waypoints;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float arriveDistance = 0.05f;
    [SerializeField] private bool loopPath = true;

    private int currentWaypointIndex;
    private int moveDirection = 1;

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("WaypointMover: Target is not assigned.");
            enabled = false;
            return;
        }

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("WaypointMover: Waypoint list is empty.");
            enabled = false;
            return;
        }

        target.position = waypoints[0].position;
        currentWaypointIndex = 0;
    }

    private void Update()
    {
        MoveTarget();
    }

    private void MoveTarget()
    {
        Transform currentWaypoint = waypoints[currentWaypointIndex];

        target.position = Vector3.MoveTowards(
            target.position,
            currentWaypoint.position,
            moveSpeed * Time.deltaTime
        );

        float distanceToWaypoint = Vector3.Distance(
            target.position,
            currentWaypoint.position
        );

        if (distanceToWaypoint > arriveDistance)
            return;

        SelectNextWaypoint();
    }

    private void SelectNextWaypoint()
    {
        if (loopPath)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

            return;
        }

        currentWaypointIndex += moveDirection;

        if (currentWaypointIndex >= waypoints.Length)
        {
            currentWaypointIndex = waypoints.Length - 2;
            moveDirection = -1;
        }
        else if (currentWaypointIndex < 0)
        {
            currentWaypointIndex = 1;
            moveDirection = 1;
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null)
                continue;

            Gizmos.DrawSphere(waypoints[i].position, 0.1f);

            int nextIndex = i + 1;

            if (nextIndex >= waypoints.Length)
            {
                if (!loopPath)
                    continue;

                nextIndex = 0;
            }

            if (waypoints[nextIndex] == null)
                continue;

            Gizmos.DrawLine(
                waypoints[i].position,
                waypoints[nextIndex].position
            );
        }
    }
}