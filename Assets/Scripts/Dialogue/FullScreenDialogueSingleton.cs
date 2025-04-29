using System;
using System.Collections.Generic;
using EasyTextEffects;
using FriedSynapse.FlowEnt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FullScreenDialogueSingleton : AbstractMonoBehaviourSingleton<FullScreenDialogueSingleton>
{
    [SerializeField]
    private TMP_Text dialogue;
    private TMP_Text Dialogue => dialogue;
    [SerializeField]
    private CanvasGroup dialogueGroup;
    private CanvasGroup DialogueGroup => dialogueGroup;
    [SerializeField]
    private InputActionReference activate;
    private InputActionReference Activate => activate;
    [SerializeField]
    private CanvasGroup promptGroup;
    private CanvasGroup PromptGroup => promptGroup;

    private AbstractAnimation ShowHideAnimation { get; set; }

    private Queue<DialogueData> DialogueQueue { get; set; }
    private TMP_CharacterQueue CharacterQueue { get; set; }
    private bool IsReadyForNext { get; set; }
    private float TimeElapsed { get; set; }
    private SpeakerSO PreviousSpeaker { get; set; }

    public Action OnSectionCompleted { get; set; }

    private const float TimeBetweenCharacters = 0.03f;

    void Update()
    {

        if (Activate.action.WasPressedThisFrame())
        {
            if (IsReadyForNext)
            {
                PlayNextDialogue();
            }
        }
        else if (!IsReadyForNext && CharacterQueue != null)
        {
            if (TimeElapsed >= TimeBetweenCharacters)
            {
                TimeElapsed = 0;
                NextCharacter();
            }
            else
            {
                TimeElapsed += Time.deltaTime;
            }
        }
    }

    public void Show()
    {
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(DialogueGroup)
            .AlphaTo(1)
            .SetEasing(Easing.EaseOutSine)
            .OnCompleted(() =>
            {
                PlayNextDialogue();
            })
            .Start();
    }

    public void Hide()
    {
        PlayerController.Instance.UnlockControls();
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(DialogueGroup)
            .AlphaTo(0)
            .SetEasing(Easing.EaseInSine)
            .OnCompleted(() =>
            {
                Debug.Log("Calling on section completed");
                Dialogue.text = string.Empty;
                OnSectionCompleted?.Invoke();
            })
            .Start();
    }
    public void EnqueueDialogue(DialogueSectionSO dialogueSection)
    {
        PreviousSpeaker = null;
        if (DialogueQueue == null)
        {
            DialogueQueue = new Queue<DialogueData>();
        }
        foreach (var dialogueData in dialogueSection.DialogueData)
        {
            DialogueQueue.Enqueue(dialogueData);
        }

        PlayerController.Instance.LockControls();
        Show();
    }

    private void PlayNextDialogue()
    {
        IsReadyForNext = false;
        PromptGroup.alpha = 0;
        if (DialogueQueue.Count > 0)
        {
            var dialogue = DialogueQueue.Dequeue();
            if (PreviousSpeaker == dialogue.Speaker)
            {
                AddText(dialogue.Dialogue);
            }
            else
            {
                AddText(dialogue.ToTaggedString());
            }
            PreviousSpeaker = dialogue.Speaker;

        }
        else
        {
            Hide();
        }
    }

    private void AddText(string text)
    {
        CharacterQueue = new TMP_CharacterQueue($"{text}{Environment.NewLine}{Environment.NewLine}");
    }

    void NextCharacter()
    {
        bool isSuccessful = CharacterQueue.TryGetNextCharacter(out string characterString);
        if (!isSuccessful)
        {
            CharacterQueue = null;
            IsReadyForNext = true;
            PromptGroup.alpha = 1;
            return;
        }

        Dialogue.text += characterString;
    }

}
