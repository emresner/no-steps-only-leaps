using Unity.Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private CinemachineImpulseSource impulseSource;

    [Header("Shake Forces")]
    [SerializeField] private float trapHitShakeForce = 0.45f;
    [SerializeField] private float hardLandingShakeForce = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (impulseSource == null)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }
    }

    public void ShakeTrapHit()
    {
        Shake(trapHitShakeForce);
    }

    public void ShakeHardLanding()
    {
        Shake(hardLandingShakeForce);
    }

    private void Shake(float force)
    {
        if (impulseSource == null)
            return;

        impulseSource.GenerateImpulseWithForce(force);
    }
}