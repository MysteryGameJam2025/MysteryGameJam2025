using UnityEngine;

public class PuzzleRoomThreeController : MonoBehaviour
{
    [SerializeField]
    private Symbol ruin;
    private Symbol Ruin => ruin;
    [SerializeField]
    private Symbol fulfill;
    private Symbol Fulfill => fulfill;
    [SerializeField]
    private Symbol entrust;
    private Symbol Entrust => entrust;
    [SerializeField]
    private StairsController stairs;
    private StairsController Stairs => stairs;
    [SerializeField]
    private CrystalController crystal;
    private CrystalController Crystal => crystal;

    private bool IsStairsDestroyed { get; set; }
    private bool IsBrokenEmitterFixed { get; set; }
    private bool IsCrystalDropped { get; set; }

    public void OnStairsSymbolPlateUsed(Symbol symbol)
    {
        if (!IsStairsDestroyed && symbol == Ruin)
        {
            Stairs.DestroyRealStairs();
            IsStairsDestroyed = true;
        }

        if (IsStairsDestroyed && symbol == Fulfill)
        {
            Stairs.SetHologramStarisEnabled(true);
        }
        else
        {
            Stairs.SetHologramStarisEnabled(false);
        }
    }

    public void OnEmitterSymbolPlateUsed(Symbol symbol)
    {
        if (!IsCrystalDropped && symbol == Ruin)
        {
            IsCrystalDropped = true;
            Crystal.DropCrystal();
        }
    }

    public void OnEmitterFixed()
    {
        IsBrokenEmitterFixed = true;
    }

}
