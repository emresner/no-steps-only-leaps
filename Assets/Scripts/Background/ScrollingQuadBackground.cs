using UnityEngine;

public class ScrollingQuadBackground : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Renderer targetRenderer;

    [Header("Scroll Settings")]
    [SerializeField] private Vector2 scrollSpeed = new Vector2(0.02f, 0.01f);

    private Material materialInstance;
    private Vector2 currentOffset;

    private void Awake()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        materialInstance = targetRenderer.material;
    }

    private void Update()
    {
        currentOffset += scrollSpeed * Time.deltaTime;
        currentOffset.x %= 1f;
        currentOffset.y %= 1f;

        SetTextureOffset(currentOffset);
    }

    private void SetTextureOffset(Vector2 offset)
    {
        if (materialInstance.HasProperty("_BaseMap"))
        {
            materialInstance.SetTextureOffset("_BaseMap", offset);
            return;
        }

        if (materialInstance.HasProperty("_MainTex"))
        {
            materialInstance.SetTextureOffset("_MainTex", offset);
        }
    }
}