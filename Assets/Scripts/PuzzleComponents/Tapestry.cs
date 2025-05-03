using System.Collections.Generic;
using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.Events;

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
    private Sprite tapestryImage;
    private Sprite TapestryImage => tapestryImage;
    [SerializeField]
    private UnityEvent onSymbolsSet;
    private UnityEvent OnSymbolsSet => onSymbolsSet;


    private AbstractAnimation PromptAnimation { get; set; }

    public override void OnInteract(InteractionEvent e)
    {
        // TODO: Implement tapestry image display
        // Show tapestry image
        // On close of image, set symbols
        GauntletController.Instance.SetSymbols(SymbolsToEquip);
        OnSymbolsSet?.Invoke();
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
}
