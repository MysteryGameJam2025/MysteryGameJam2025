using System;
using FriedSynapse.FlowEnt;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class GauntletUIController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;
    private CanvasGroup CanvasGroup => canvasGroup;
    [SerializeField]
    private Image symbolUIImage;
    public Image SymbolUIImage => symbolUIImage;
    [SerializeField]
    private Image fromRight;
    private Image FromRight => fromRight;
    [SerializeField]
    private Image fromLeft;
    private Image FromLeft => fromLeft;
    [SerializeField]
    private RectTransform pivot;
    private RectTransform Pivot => pivot;
    [SerializeField]
    private RectTransform leftArrow;
    private RectTransform LeftArrow => leftArrow;
    [SerializeField]
    private RectTransform rightArrow;
    private RectTransform RightArrow => rightArrow;

    private AbstractAnimation ShowHideAnimation { get; set; }
    private AbstractAnimation DialAnimation { get; set; }
    public bool IsAnimationPlaying { get; private set; }

    public void SetInitialSymbol(Symbol symbol)
    {
        SymbolUIImage.sprite = symbol.SymbolSprite;
    }

    public void SetSymbolInUI(Symbol symbol, bool isNext)
    {
        (isNext ? FromRight : FromLeft).sprite = symbol.SymbolSprite;

        DialAnimation?.Stop();
        IsAnimationPlaying = true;
        DialAnimation = new Flow()
            .Queue(new Tween(1)
                .For(Pivot)
                .RotateLocalZTo(isNext ? 90 : -90)
                .SetEasing(Easing.EaseInOutSine))
            .At(0, new Tween(0.5f)
                .For(isNext ? RightArrow : LeftArrow)
                .MoveAnchoredPositionX(isNext ? 20 : -20)
                .SetEasing(Easing.EaseInSine))
            .At(0.5f, new Tween(0.5f)
                .For(isNext ? RightArrow : LeftArrow)
                .MoveAnchoredPositionX(isNext ? -20 : 20)
                .SetEasing(Easing.EaseOutSine))
            .OnCompleted(() =>
                {
                    IsAnimationPlaying = false;
                    SymbolUIImage.sprite = symbol.SymbolSprite;
                    Pivot.rotation = Quaternion.Euler(0, 0, 0);
                })
            .Start();
    }

    public void Show()
    {
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(CanvasGroup)
            .AlphaTo(1)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }

    public void Hide()
    {
        ShowHideAnimation?.Stop();
        ShowHideAnimation = new Tween(1)
            .For(CanvasGroup)
            .AlphaTo(0)
            .SetEasing(Easing.EaseOutSine)
            .Start();
    }
}
