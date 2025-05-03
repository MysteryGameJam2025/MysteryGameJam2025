using FriedSynapse.FlowEnt;
using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OverarchingNoteController : AbstractMonoBehaviourSingleton<OverarchingNoteController>
{
    [SerializeField]
    private TMP_Text textField;
    private TMP_Text TextField => textField;

    [SerializeField]
    private CanvasGroup canvasGroup;
    private CanvasGroup CanvasGroup => canvasGroup;

    [SerializeField]
    private Button closeButton;
    private Button CloseButton => closeButton;

    private Tween ShowHideTween;
    private Action OnNoteClose;

    private void Start()
    {
        CloseButton.onClick.AddListener(HideNote);
    }

    public void ShowNote(Symbol symbolToReplace = null, Action onNoteClose = null)
    {
        OnNoteClose = onNoteClose;
        if (symbolToReplace != null)
            ReplaceSymbolWithText(symbolToReplace.SymbolName);

        ShowNote();
    }

    private void ShowNote()
    {
        ShowHideTween?.Stop();
        ShowHideTween = new Tween(0.5f)
            .OnStarted(() =>
            {
                PlayerController.Instance?.LockControls();
            })
            .For(CanvasGroup)
                .AlphaTo(0, 1)
            .SetEasing(Easing.EaseInCubic)
            .OnCompleted(() =>
            {
                CanvasGroup.interactable = true;
                CanvasGroup.blocksRaycasts = true;
            })
            .Start();
    }

    private void HideNote()
    {
        ShowHideTween?.Stop();
        ShowHideTween = new Tween(0.5f)
            .OnStarted(() =>
            {
                CanvasGroup.interactable = false;
                CanvasGroup.blocksRaycasts = false;
            })
            .For(CanvasGroup)
                .AlphaTo(1, 0)
            .SetEasing(Easing.EaseOutCubic)
            .OnCompleted(() =>
            {
                PlayerController.Instance?.UnlockControls();
                OnNoteClose?.Invoke();
            })
            .Start();
    }

    private void ReplaceSymbolWithText(string symbolName)
    {
        string original = TextField.text;
        string newText = "";

        Regex regex = new Regex($"<sprite name={symbolName}>");
        newText = regex.Replace(original, symbolName);

        TextField.text = newText;
    }
}
