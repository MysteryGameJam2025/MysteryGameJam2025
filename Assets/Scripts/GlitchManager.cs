using FriedSynapse.FlowEnt;
using UnityEngine;

public class GlitchManager : AbstractMonoBehaviourSingleton<GlitchManager>
{
    [SerializeField]
    private Material glitchMaterial;
    private Material GlitchMaterial => glitchMaterial;

    private const string EffectStrengthRef = "_EffectStrength";

    private AbstractAnimation GlitchAnim { get; set; }


    void Awake()
    {
        GlitchMaterial.SetFloat(EffectStrengthRef, 0);
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

    public void PlaySlowBuildup()
    {
        GlitchAnim?.Stop();
        GlitchAnim = new Tween(10)
            .For(GlitchMaterial)
            .FloatTo(EffectStrengthRef, 1)
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
}
