using UnityEngine;

public class DoorControl : SymbolActivatableBase
{
    private bool powered;
    private bool Powered => powered;

    [SerializeField]
    private GameObject door;
    public GameObject Door => door;

    public void SetPowered(bool isPowered)
    {
        powered = isPowered;
    }
}
