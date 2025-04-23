using EasyTextEffects;
using FriedSynapse.FlowEnt;
using TMPro;
using UnityEngine;

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

    private AbstractAnimation ShowHideAnimation { get; set; }

    public void Show()
    {
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(DialogueGroup)
            .AlphaTo(1)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }

    public void Hide()
    {
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(DialogueGroup)
            .AlphaTo(0)
            .SetEasing(Easing.EaseInSine)
            .Start();
    }

    public void SetText(string text)
    {
        Dialogue.text = text;
        TypewriterEffect.StartManualEffects();
    }
}
