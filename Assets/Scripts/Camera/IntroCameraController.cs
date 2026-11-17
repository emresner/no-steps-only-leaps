using System.Collections;
using UnityEngine;

public class IntroCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject introCameraObject;
    [SerializeField] private Transform introCameraTarget;
    [SerializeField] private Transform introStartPoint;
    [SerializeField] private Transform introEndPoint;

    [Header("Settings")]
    [SerializeField] private float introDuration = 3.5f;

    public void PrepareIntroCamera()
    {
        if (!HasRequiredReferences())
            return;

        introCameraTarget.position = GetStartPosition();

        if (introCameraObject != null)
        {
            introCameraObject.SetActive(true);
        }
    }

    public IEnumerator PlayIntro()
    {
        if (!HasRequiredReferences())
        {
            HideIntroCamera();
            yield break;
        }

        PrepareIntroCamera();

        Vector3 startPosition = GetStartPosition();
        Vector3 endPosition = GetEndPosition();

        float timer = 0f;

        while (timer < introDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / introDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            introCameraTarget.position = Vector3.Lerp(
                startPosition,
                endPosition,
                t
            );

            yield return null;
        }

        introCameraTarget.position = endPosition;

        HideIntroCamera();
    }

    public void HideIntroCamera()
    {
        if (introCameraObject != null)
        {
            introCameraObject.SetActive(false);
        }
    }

    private Vector3 GetStartPosition()
    {
        Vector3 startPosition = introEndPoint.position;
        startPosition.y = introStartPoint.position.y;
        startPosition.z = introCameraTarget.position.z;

        return startPosition;
    }

    private Vector3 GetEndPosition()
    {
        Vector3 endPosition = introEndPoint.position;
        endPosition.z = introCameraTarget.position.z;

        return endPosition;
    }

    private bool HasRequiredReferences()
    {
        return introCameraObject != null &&
               introCameraTarget != null &&
               introStartPoint != null &&
               introEndPoint != null;
    }
}