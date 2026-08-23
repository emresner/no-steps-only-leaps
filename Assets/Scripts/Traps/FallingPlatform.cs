using System.Collections;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform visual;
    [SerializeField] private Collider2D platformCollider;

    [Header("Timing")]
    [SerializeField] private float warningDuration = 0.5f;
    [SerializeField] private float fallDuration = 0.6f;
    [SerializeField] private float respawnDelay = 2f;

    [Header("Fall")]
    [SerializeField] private float fallDistance = 4f;
    [SerializeField] private bool keepColliderDuringFall = true;

    [Header("Shake")]
    [SerializeField] private float shakeAmount = 0.04f;
    [SerializeField] private float shakeRotationAmount = 3f;
    [SerializeField] private float shakeSpeed = 35f;

    private Vector3 startPosition;
    private Vector3 visualStartLocalPosition;
    private Quaternion visualStartLocalRotation;

    private bool isTriggered;

    private void Awake()
    {
        startPosition = transform.position;

        if (visual != null)
        {
            visualStartLocalPosition = visual.localPosition;
            visualStartLocalRotation = visual.localRotation;
        }

        if (platformCollider == null)
            platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isTriggered)
            return;

        Player player = collision.collider.GetComponentInParent<Player>();

        if (player == null)
            return;

        StartCoroutine(FallSequence());
    }

    private IEnumerator FallSequence()
    {
        isTriggered = true;

        SetVisualActive(true);

        if (platformCollider != null)
            platformCollider.enabled = true;

        yield return ShakeWarning();

        if (!keepColliderDuringFall && platformCollider != null)
            platformCollider.enabled = false;

        yield return FallDown();

        SetVisualActive(false);

        if (platformCollider != null)
            platformCollider.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        Respawn();
    }

    private IEnumerator ShakeWarning()
    {
        float timer = 0f;

        while (timer < warningDuration)
        {
            timer += Time.deltaTime;

            if (visual != null)
            {
                float shakeX = Mathf.Sin(timer * shakeSpeed) * shakeAmount;
                float shakeRotation = Mathf.Sin(timer * shakeSpeed) * shakeRotationAmount;

                visual.localPosition = visualStartLocalPosition + new Vector3(shakeX, 0f, 0f);
                visual.localRotation = Quaternion.Euler(0f, 0f, shakeRotation);
            }

            yield return null;
        }

        ResetVisual();
    }

    private IEnumerator FallDown()
    {
        Vector3 from = transform.position;
        Vector3 to = startPosition + Vector3.down * fallDistance;

        float timer = 0f;

        while (timer < fallDuration)
        {
            timer += Time.deltaTime;

            float t = timer / fallDuration;
            t = Mathf.Clamp01(t);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.position = Vector3.Lerp(from, to, t);

            yield return null;
        }

        transform.position = to;
    }

    private void Respawn()
    {
        transform.position = startPosition;

        ResetVisual();
        SetVisualActive(true);

        if (platformCollider != null)
            platformCollider.enabled = true;

        isTriggered = false;
    }

    private void ResetVisual()
    {
        if (visual == null)
            return;

        visual.localPosition = visualStartLocalPosition;
        visual.localRotation = visualStartLocalRotation;
    }

    private void SetVisualActive(bool isActive)
    {
        if (visual == null)
            return;

        visual.gameObject.SetActive(isActive);
    }
}