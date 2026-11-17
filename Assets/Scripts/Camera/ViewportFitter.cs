using UnityEngine;

public class ViewportFitter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera targetCamera;

    [Header("Viewport Settings")]
    [SerializeField] private float targetAspect = 16f / 9f;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Awake()
    {
        if (targetCamera == null)
        {
            targetCamera = GetComponent<Camera>();
        }
    }

    private void OnEnable()
    {
        ApplyViewport();
    }

    private void Update()
    {
        if (Screen.width == lastScreenWidth && Screen.height == lastScreenHeight)
            return;

        ApplyViewport();
    }

    private void ApplyViewport()
    {
        if (targetCamera == null)
            return;

        if (Screen.width <= 0 || Screen.height <= 0)
            return;

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        float screenAspect = (float)Screen.width / Screen.height;
        Rect viewport = new Rect(0f, 0f, 1f, 1f);

        if (screenAspect > targetAspect)
        {
            viewport.width = targetAspect / screenAspect;
            viewport.x = (1f - viewport.width) * 0.5f;
        }
        else
        {
            viewport.height = screenAspect / targetAspect;
            viewport.y = (1f - viewport.height) * 0.5f;
        }

        targetCamera.rect = viewport;
    }
}
