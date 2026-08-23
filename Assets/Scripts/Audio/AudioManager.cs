using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Master Volume")]
    [Range(0f, 1f)]
    [SerializeField] private float masterSfxVolume = 1f;

    [Header("UI SFX")]
    [SerializeField] private AudioClip buttonClickClip;
    [Range(0f, 1f)]
    [SerializeField] private float buttonClickVolume = 0.8f;

    [Header("Player SFX")]
    [SerializeField] private AudioClip jumpClip;
    [Range(0f, 1f)]
    [SerializeField] private float jumpVolume = 0.8f;

    [SerializeField] private AudioClip wallBounceClip;
    [Range(0f, 1f)]
    [SerializeField] private float wallBounceVolume = 0.8f;

    [SerializeField] private AudioClip trapHitClip;
    [Range(0f, 1f)]
    [SerializeField] private float trapHitVolume = 0.9f;

    [Header("World SFX")]
    [SerializeField] private AudioClip collectiblePickupClip;
    [Range(0f, 1f)]
    [SerializeField] private float collectiblePickupVolume = 0.8f;

    [SerializeField] private AudioClip checkpointActivateClip;
    [Range(0f, 1f)]
    [SerializeField] private float checkpointActivateVolume = 0.9f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public void PlayButtonClick()
    {
        PlaySfx(buttonClickClip, buttonClickVolume);
    }

    public void PlayJump()
    {
        PlaySfx(jumpClip, jumpVolume);
    }

    public void PlayWallBounce()
    {
        PlaySfx(wallBounceClip, wallBounceVolume);
    }

    public void PlayTrapHit()
    {
        PlaySfx(trapHitClip, trapHitVolume);
    }

    public void PlayCollectiblePickup()
    {
        PlaySfx(collectiblePickupClip, collectiblePickupVolume);
    }

    public void PlayCheckpointActivate()
    {
        PlaySfx(checkpointActivateClip, checkpointActivateVolume);
    }

    private void PlaySfx(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        if (sfxSource == null)
            return;

        sfxSource.PlayOneShot(clip, masterSfxVolume * volume);
    }
}