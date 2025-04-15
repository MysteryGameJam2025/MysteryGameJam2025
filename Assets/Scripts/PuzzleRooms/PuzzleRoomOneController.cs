using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PuzzleRoomOneController : MonoBehaviour
{
    [SerializeField]
    private Symbol attraction;
    private Symbol Attraction => attraction;

    [SerializeField]
    private Symbol connection;
    private Symbol Connection => connection;

    [SerializeField]
    private Symbol energy;
    private Symbol Energy => energy;

    [SerializeField]
    private PowerSphereController powerSphere;
    private PowerSphereController PowerSphere => powerSphere;

    [SerializeField]
    private BackstopPuzzleComponent backstop;
    private BackstopPuzzleComponent Backstop => backstop;

    [SerializeField]
    private DoorControl doorControls;
    private DoorControl DoorControls => doorControls;

    bool isSphereMoved;

    bool areControlsConnected;


    public void SetSymbolBottomOfHill(Symbol symbol)
    {
        if (isSphereMoved)
        {
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);
        CheckSphereAndBackstop();
    }

    public void SetSymbolTopOfHill(Symbol symbol)
    {
        if (!isSphereMoved)
        {
            Backstop.SetCurrentSymbol(symbol);
            CheckSphereAndBackstop();
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);
        CheckSphereAndDoorControls();
    }

    public void SetDoorSymbol(Symbol symbol)
    {
        DoorControls.SetCurrentSymbol(symbol);
        CheckSphereAndDoorControls();
    }

    public void CheckSphereAndBackstop()
    {
        if (PowerSphere.CurrentSymbol == Attraction && Backstop.CurrentSymbol == Attraction)
        {
            PowerSphere.SetTarget(backstop);
            isSphereMoved = true;
        }
    }

    public void CheckSphereAndDoorControls()
    {
        if (PowerSphere.CurrentSymbol == Connection && DoorControls.CurrentSymbol == Connection)
        {
            areControlsConnected = true;
        }
        else if (PowerSphere.CurrentSymbol == Energy && DoorControls.CurrentSymbol == Energy && areControlsConnected)
        {
            DoorControls.Open();
        }
    }
}
