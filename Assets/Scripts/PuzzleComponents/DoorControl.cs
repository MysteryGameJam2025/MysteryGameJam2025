using UnityEngine;

public class DoorControl : SymbolActivatableBase
{
    private bool powered;
    private bool Powered => powered;

    [SerializeField]
    private GameObject door;
    public GameObject Door => door;

    public override void OnSymbolInteract(Symbol symbol, GauntletController gauntlet)
    {
        currentSymbol = symbol;
        if (Powered)
        {
            Destroy(Door);
        }
    }

    public void SetPowered(bool isPowered)
    {
        powered = isPowered;
    }
}
