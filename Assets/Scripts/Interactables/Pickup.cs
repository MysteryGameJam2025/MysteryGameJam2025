using FriedSynapse.FlowEnt;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : AbstractInteractable
{
    [SerializeField]
    private UnityEvent onPickup;
    private UnityEvent OnPickup => onPickup;
    [SerializeField]
    private CanvasGroup promptDisplay;
    private CanvasGroup PromptDisplay => promptDisplay;

    private AbstractAnimation PromptAnimation { get; set; }


    public override void OnInteract(InteractionEvent e)
    {
        OnPickup.Invoke();
        Destroy(gameObject);
    }

    public override void OnInteractionHoverStart()
    {
        if (PromptDisplay == null)
        {
            return;
        }

        PromptAnimation?.Stop();
        PromptAnimation = new Tween(0.6f)
            .For(PromptDisplay)
            .AlphaTo(1)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }

    public override void OnInteractionHoverEnd()
    {
        if (PromptDisplay == null)
        {
            return;
        }

        PromptAnimation?.Stop();
        PromptAnimation = new Tween(0.6f)
            .For(PromptDisplay)
            .AlphaTo(0)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }

    void OnDestroy()
    {
        PromptAnimation?.Stop();
    }
}
