using FriedSynapse.FlowEnt;
using System;
using UnityEngine;

public class GlitchManager : AbstractMonoBehaviourSingleton<GlitchManager>
{
    [SerializeField]
    private Material glitchMaterial;
    private Material GlitchMaterial => glitchMaterial;

    private const string EffectStrengthRef = "_EffectStrength";
    private const string PosterizeStrengthRef = "_PosterizeStrength";

    private float PosterizeStrengthStart;

    private AbstractAnimation GlitchAnim { get; set; }


    void Awake()
    {
        GlitchMaterial.SetFloat(EffectStrengthRef, 0);
        PosterizeStrengthStart = GlitchMaterial.GetFloat(PosterizeStrengthRef);
    }

    public void PlayTinyGlitch()
    {
        GlitchInOut(1, 0.5f);
    }

    [EasyButtons.Button]
    public void PlayShortGlitch()
    {
        GlitchInOut(1, 1);
    }

    [EasyButtons.Button]
    public void PlayFullGlitchOut(Action onAtFullStrength = null)
    {
        PlayFullGlitchOut(5, 1, onAtFullStrength);
    }

    public void PlaySlowBuildup()
    {
        GlitchAnim?.Stop();
        GlitchAnim = new Tween(10)
            .For(GlitchMaterial)
            .FloatTo(EffectStrengthRef, 0, 1)
            .SetEasing(Easing.EaseInQuint)
            .Start();
    }

    private void PlayFullGlitchOut(float duration, float strength, Action onAtFullStrength = null)
    {
        GlitchAnim?.Stop();
        GlitchAnim = new Flow()
            .Queue(new Tween(duration * 0.20f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, 0, strength)
                .For(GlitchMaterial)
                .FloatTo(PosterizeStrengthRef, PosterizeStrengthStart, 1))
            .Queue(new Tween(duration * 0.60f)
                .OnStarted(() => onAtFullStrength?.Invoke()))
            .Queue(new Tween(duration * 0.20f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, strength, 0)
                .For(GlitchMaterial)
                .FloatTo(PosterizeStrengthRef, 1, PosterizeStrengthStart))
            .Start();
    }

    private void GlitchInOut(float duration, float strength)
    {
        GlitchAnim?.Stop();
        GlitchAnim = new Flow()
            .Queue(new Tween(duration * 0.5f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, 0, strength))
            .Queue(new Tween(duration * 0.5f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, strength, 0))
            .Start();
    }

    public void StopGlitches()
    {
        GlitchMaterial.SetFloat(EffectStrengthRef, 0);
    }
}
