using UnityEngine;
using UnityEngine.UI;

public class JumpChargeFillUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Image fillImage;

    [Header("Settings")]
    [SerializeField] private Color chargingColor = new Color(0.91f, 0.45f, 0.5f, 0.75f);
    [SerializeField] private Color fullColor = new Color(1f, 0.92f, 0.55f, 0.9f);

    private void Update()
    {
        if (player == null || fillImage == null)
            return;

        float percent = Mathf.Clamp01(player.ChargePercent);

        fillImage.fillAmount = percent;
        fillImage.color = percent >= 1f ? fullColor : chargingColor;
    }
}
