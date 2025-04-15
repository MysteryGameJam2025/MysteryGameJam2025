using UnityEngine;

public class DoorControl : SymbolActivatableBase
{
    [SerializeField]
    private GameObject door;
    public GameObject Door => door;

    public void Open()
    {
        Destroy(Door);
    }
}
