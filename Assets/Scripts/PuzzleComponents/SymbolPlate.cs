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
    private AbstractAnimation PlateAnimation { get; set; }
    private Material SymbolMaterial { get; set; }

    private bool HasSymbol { get; set; }

    void Awake()
    {
        HasSymbol = false;
        SymbolMaterial = new Material(SymbolDisplay.material);
        SymbolDisplay.material = SymbolMaterial;
    }

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

    public void HidePrompt()
    {
        PromptAnimation?.Stop();
        PromptCanvas.alpha = 0;

    }

    public override void OnInteract(InteractionEvent e)
    {
        Debug.Log("plate interact");
        PlateAnimation?.Stop();
        Flow flow = new Flow();
        //flow.QueueDelay(1);
        if (HasSymbol)
        {
            flow.Queue(new Tween(1)
            .For(SymbolMaterial)
            .FloatTo("_Progress", 1, 0)
            .OnCompleted(() =>
            {
                SymbolDisplay.sprite = e.EquippedSymbol.SymbolSprite;
            }));
        }
        else
        {
            SymbolDisplay.sprite = e.EquippedSymbol.SymbolSprite;
        }

        HasSymbol = true;
        PlateAnimation = flow
            .Queue(new Tween(1)
            .For(SymbolMaterial)
            .FloatTo("_Progress", 0, 1))
            .OnCompleted(() =>
            {
                OnSymbolUsed.Invoke(e.EquippedSymbol);
            })
            .Start();


    }


}
