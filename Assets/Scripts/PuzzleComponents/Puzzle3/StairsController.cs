using FriedSynapse.FlowEnt;
using UnityEngine;

public class StairsController : MonoBehaviour
{
    [SerializeField]
    private GameObject realStairs;
    private GameObject RealStairs => realStairs;
    [SerializeField]
    private GameObject hologramStairs;
    private GameObject HologramStairs => hologramStairs;
    [SerializeField]
    private Material stairsMat;
    private Material StairsMat => stairsMat;
    [SerializeField]
    private float cutoffStart;
    private float CutoffStart => cutoffStart;
    [SerializeField]
    private float cutoffEnd;
    private float CutoffEnd => cutoffEnd;

    private AbstractAnimation DestroyAnimation { get; set; }

    private const string CutoffHeightString = "_Cuttoff_Height";

    void Awake()
    {
        StairsMat.SetFloat(CutoffHeightString, CutoffStart);
    }

    public void DestroyRealStairs()
    {
        DestroyAnimation?.Stop();
        DestroyAnimation = new Tween(4)
            .For(StairsMat)
            .FloatTo(CutoffHeightString, CutoffStart, CutoffEnd)
            .OnCompleted(() =>
            {
                RealStairs.SetActive(false);
            })
            .Start();

    }

    public void SetHologramStarisEnabled(bool isEnabled)
    {
        HologramStairs.SetActive(isEnabled);
    }
}
