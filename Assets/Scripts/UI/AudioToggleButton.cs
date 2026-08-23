using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggleButton : MonoBehaviour
{
    private const string AudioMutedKey = "audio_muted";

    [Header("References")]
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text audioText;

    [Header("Text")]
    [SerializeField] private string audioOnText = "AUDIO ON";
    [SerializeField] private string audioOffText = "AUDIO OFF";

    [Header("Icon Colors")]
    [SerializeField] private Color audioOnColor = Color.white;
    [SerializeField] private Color audioOffColor = new Color(1f, 1f, 1f, 0.45f);

    private bool isMuted;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (iconImage == null)
            iconImage = GetComponent<Image>();

        if (audioText == null)
            audioText = GetComponentInChildren<TMP_Text>(true);

        if (button != null)
            button.onClick.AddListener(ToggleAudio);
    }

    private void Start()
    {
        LoadAudioState();
        ApplyAudioState();
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(ToggleAudio);
    }

    private void ToggleAudio()
    {
        isMuted = !isMuted;

        SaveAudioState();
        ApplyAudioState();
    }

    private void LoadAudioState()
    {
        isMuted = PlayerPrefs.GetInt(AudioMutedKey, 0) == 1;
    }

    private void SaveAudioState()
    {
        PlayerPrefs.SetInt(AudioMutedKey, isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyAudioState()
    {
        AudioListener.volume = isMuted ? 0f : 1f;

        if (audioText != null)
            audioText.text = isMuted ? audioOffText : audioOnText;

        if (iconImage != null)
            iconImage.color = isMuted ? audioOffColor : audioOnColor;
    }
}