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
    [SerializeField]
    private GameObject bottomCrystal;
    private GameObject BottomCrystal => bottomCrystal;
    [SerializeField]
    private GameObject holoCrystal;
    private GameObject HoloCrystal => holoCrystal;

    private bool IsStairsDestroyed { get; set; }
    private bool IsBrokenEmitterFixed { get; set; }
    private bool IsCrystalDropped { get; set; }
    private bool IsHoloCrystalActive { get; set; }

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

        if (IsCrystalDropped && symbol == Fulfill)
        {
            IsHoloCrystalActive = true;
            HoloCrystal.SetActive(true);
        }

        if (IsBrokenEmitterFixed && IsHoloCrystalActive)
        {
            Debug.Log("You win!");
        }
    }

    public void OnEmitterFixed()
    {
        IsBrokenEmitterFixed = true;
        Crystal.gameObject.SetActive(false);
        BottomCrystal.SetActive(true);
    }

}
