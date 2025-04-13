using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using FriedSynapse.FlowEnt;

public class SymbolPlate : AbstractInteractable
{
    [SerializeField]
    private Image symbolDisplay;
    private Image SymbolDisplay => symbolDisplay;
    [SerializeField]
    private CanvasGroup promptCanvas;
    private CanvasGroup PromptCanvas => promptCanvas;
    public UnityEvent<Symbol> OnSymbolUsed;

    private AbstractAnimation PromptAnimation { get; set; }

    public override void OnInteractionHoverStart()
    {
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
            .Start();
    }

    public override void OnInteract()
    {
        throw new System.NotImplementedException();
    }


}
