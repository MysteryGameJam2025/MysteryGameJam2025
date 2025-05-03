using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tapestry : AbstractInteractable
{
    [SerializeField]
    private GameObject particle;
    private GameObject Particle => particle;
    [SerializeField]
    private CanvasGroup promptCanvas;
    private CanvasGroup PromptCanvas => promptCanvas;
    [SerializeField]
    private List<Symbol> symbolsToEquip;
    private List<Symbol> SymbolsToEquip => symbolsToEquip;
    [SerializeField]
    private UnityEvent onSymbolsSet;
    private UnityEvent OnSymbolsSet => onSymbolsSet;

    private AbstractAnimation PromptAnimation { get; set; }

    public override void OnInteract(InteractionEvent e)
    {
        // TODO: Implement tapestry image display
        // Show tapestry image
        // On close of image, set symbols

        PlayerController.Instance.CloseButton.onClick.AddListener(CloseTapestry);
        new Tween(0.5f)
            .OnStarted(() =>
            {
                PlayerController.Instance?.LockControls();
            })
            .For(PlayerController.Instance.TapestryGroup)
                .AlphaTo(0, 1)
            .SetEasing(Easing.EaseInCubic)
            .OnCompleted(() =>
            {
                PlayerController.Instance.TapestryGroup.interactable = true;
                PlayerController.Instance.TapestryGroup.blocksRaycasts = true;
            })
            .Start();
    }

    public override void OnInteractionHoverStart()
    {
        Particle.SetActive(false);

        PromptAnimation?.Stop();
        PromptAnimation = new Tween(0.6f)
            .For(PromptCanvas)
            .AlphaTo(1)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }

    public override void OnInteractionHoverEnd()
    {
        PromptAnimation?.Stop();
        PromptAnimation = new Tween(0.6f)
            .For(PromptCanvas)
            .AlphaTo(0)
            .SetEasing(Easing.EaseOutSine)
            .OnCompleted(() =>
            {
                Particle.SetActive(true);
            })
            .Start();
    }

    private void CloseTapestry()
    {
        new Tween(0.5f)
            .OnStarted(() =>
            {
                PlayerController.Instance.TapestryGroup.interactable = false;
                PlayerController.Instance.TapestryGroup.blocksRaycasts = false;
            })
            .For(PlayerController.Instance.TapestryGroup)
                .AlphaTo(1, 0)
            .SetEasing(Easing.EaseOutCubic)
            .OnCompleted(() =>
            {
                PlayerController.Instance?.UnlockControls();
                OnTapestryClose();
            })
            .Start();
    }

    private void OnTapestryClose()
    {
        AudioController.Instance?.PlayLocalSound("GauntletActivate", PlayerController.Instance.gameObject);
        GauntletController.Instance.SetSymbols(SymbolsToEquip);
        OnSymbolsSet?.Invoke();
        Destroy(gameObject);
    }
}
