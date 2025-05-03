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

    [EasyButtons.Button]
    public void PlayShortGlitch()
    {
        GlitchInOut(1);
    }

    private void GlitchInOut(float duration)
    {
        GlitchAnim?.Stop();
        GlitchAnim = new Flow()
            .Queue(new Tween(duration * 0.5f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, 0, 1))
            .Queue(new Tween(duration * 0.5f)
                .For(GlitchMaterial)
                .FloatTo(EffectStrengthRef, 1, 0))
            .Start();
    }
}
