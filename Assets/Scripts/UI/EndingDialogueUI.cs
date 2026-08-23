using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingDialogueUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button nextButton;

    [Header("Dialogue Lines")]
    [TextArea]
    [SerializeField] private string[] dialogueLines;

    [Header("Typewriter")]
    [SerializeField] private float charactersPerSecond = 45f;

    [Header("Dialogue Sound")]
    [SerializeField] private AudioSource dialogueAudioSource;
    [SerializeField] private AudioClip dialogueBlipClip;
    [SerializeField] private int charactersPerBlip = 2;
    [SerializeField] private bool playBlipOnWhitespace = false;

    private int currentLineIndex;
    private Action dialogueCompletedCallback;

    private Coroutine typewriterCoroutine;
    private bool isTyping;

    private void Awake()
    {
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(HandleNextButtonClicked);
        }

        Hide();
    }

    private void OnDestroy()
    {
        if (nextButton != null)
        {
            nextButton.onClick.RemoveListener(HandleNextButtonClicked);
        }
    }

    public void StartDialogue(Action onCompleted)
    {
        dialogueCompletedCallback = onCompleted;
        currentLineIndex = 0;

        if (dialogueLines == null || dialogueLines.Length == 0)
        {
            CompleteDialogue();
            return;
        }

        Show();
        StartCurrentLine();
    }

    private void HandleNextButtonClicked()
    {
        if (isTyping)
        {
            CompleteCurrentLineInstantly();
            return;
        }

        ShowNextLine();
    }

    private void ShowNextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= dialogueLines.Length)
        {
            CompleteDialogue();
            return;
        }

        StartCurrentLine();
    }

    private void StartCurrentLine()
    {
        if (dialogueText == null)
            return;

        StopTypewriterCoroutine();

        string currentLine = dialogueLines[currentLineIndex];

        dialogueText.text = currentLine;
        dialogueText.maxVisibleCharacters = 0;
        dialogueText.ForceMeshUpdate();

        typewriterCoroutine = StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        isTyping = true;

        dialogueText.ForceMeshUpdate();

        int totalVisibleCharacters = dialogueText.textInfo.characterCount;
        int visibleCharacters = 0;

        float delay = 1f / Mathf.Max(1f, charactersPerSecond);

        while (visibleCharacters < totalVisibleCharacters)
        {
            visibleCharacters++;
            dialogueText.maxVisibleCharacters = visibleCharacters;

            TryPlayBlip(visibleCharacters);

            yield return new WaitForSecondsRealtime(delay);
        }

        isTyping = false;
        typewriterCoroutine = null;
    }

    private void CompleteCurrentLineInstantly()
    {
        StopTypewriterCoroutine();

        if (dialogueText == null)
            return;

        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;

        isTyping = false;
    }

    private void TryPlayBlip(int visibleCharacterIndex)
    {
        if (dialogueAudioSource == null)
            return;

        if (dialogueBlipClip == null)
            return;

        if (charactersPerBlip <= 0)
            return;

        if (visibleCharacterIndex % charactersPerBlip != 0)
            return;

        int characterInfoIndex = visibleCharacterIndex - 1;

        if (characterInfoIndex < 0 || characterInfoIndex >= dialogueText.textInfo.characterInfo.Length)
            return;

        char currentCharacter = dialogueText.textInfo.characterInfo[characterInfoIndex].character;

        if (!playBlipOnWhitespace && char.IsWhiteSpace(currentCharacter))
            return;

        dialogueAudioSource.PlayOneShot(dialogueBlipClip);
    }

    private void StopTypewriterCoroutine()
    {
        if (typewriterCoroutine == null)
            return;

        StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = null;
        isTyping = false;
    }

    private void CompleteDialogue()
    {
        StopTypewriterCoroutine();
        Hide();

        dialogueCompletedCallback?.Invoke();
        dialogueCompletedCallback = null;
    }

    private void Show()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
    }

    private void Hide()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
            dialogueText.maxVisibleCharacters = 0;
        }

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }
}