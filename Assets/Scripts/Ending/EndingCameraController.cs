using UnityEngine;

public class EndingCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform endingCameraTarget;
    [SerializeField] private GameObject endingCameraObject;

    [Header("Focus Settings")]
    [SerializeField] private float verticalOffset = 0.5f;

    public void FocusOnEnding(Transform playerTransform, Transform npcTransform)
    {
        if (endingCameraTarget == null)
            return;

        if (endingCameraObject == null)
            return;

        if (playerTransform == null || npcTransform == null)
            return;

        Vector3 focusPosition = (playerTransform.position + npcTransform.position) * 0.5f;
        focusPosition.y += verticalOffset;
        focusPosition.z = endingCameraTarget.position.z;

        endingCameraTarget.position = focusPosition;

        endingCameraObject.SetActive(true);

        Debug.Log("Ending camera focused.");
    }
}