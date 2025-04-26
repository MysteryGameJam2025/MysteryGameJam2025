using System.Collections.Generic;
using EasyTextEffects;
using FriedSynapse.FlowEnt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueSingleton : AbstractMonoBehaviourSingleton<DialogueSingleton>
{
    [SerializeField]
    private TMP_Text dialogue;
    private TMP_Text Dialogue => dialogue;
    [SerializeField]
    private CanvasGroup dialogueGroup;
    private CanvasGroup DialogueGroup => dialogueGroup;
    [SerializeField]
    private TextEffect typewriterEffect;
    private TextEffect TypewriterEffect => typewriterEffect;
    [SerializeField]
    private InputActionReference activate;
    private InputActionReference Activate => activate;
    [SerializeField]
    private CanvasGroup promptGroup;
    private CanvasGroup PromptGroup => promptGroup;

    private AbstractAnimation ShowHideAnimation { get; set; }

    private Queue<DialogueData> DialogueQueue { get; set; }
    private bool IsReadyForNext { get; set; }

    void Awake()
    {
        DialogueQueue = new Queue<DialogueData>();
    }

    void Update()
    {
        if (Activate.action.WasPressedThisFrame())
        {
            if (IsReadyForNext)
            {
                PlayNextDialogue();
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
                Dialogue.text = string.Empty;
            })
            .Start();
    }

    public void OnTypewriterFinished()
    {
        Color color = Dialogue.color;
        color.a = 1;
        Dialogue.color = color;
        IsReadyForNext = true;
        PromptGroup.alpha = 1;
    }

    public void EnqueueDialogue(DialogueSectionSO dialogueSection)
    {
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
            SetText(dialogue.ToTaggedString());
        }
        else
        {
            Hide();
        }
    }

    private void SetText(string text)
    {
        Color color = Dialogue.color;
        color.a = 0;
        Dialogue.color = color;
        Dialogue.text = text;
        TypewriterEffect.Refresh();
    }
}
