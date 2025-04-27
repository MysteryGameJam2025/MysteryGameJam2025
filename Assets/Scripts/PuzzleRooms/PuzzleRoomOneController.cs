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

    bool isSphereAtBottom = true;
    bool isSphereAtTop;
    bool areControlsConnected;
    bool areLightsOn;


    public void SetSymbolBottomOfHill(Symbol symbol)
    {
        if (!isSphereAtBottom)
        {
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);
        CheckSphereAndBackstop();
    }

    public void SetSymbolTopOfHill(Symbol symbol)
    {
        if (!isSphereAtTop)
        {
            Backstop.SetCurrentSymbol(symbol);
            CheckSphereAndBackstop();
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);

        if (PowerSphere.IsEnergised && !areLightsOn)
            TurnLightsOn();

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
            isSphereAtBottom = false;
            PowerSphere.OnReachedTarget = () => 
            { 
                isSphereAtTop = true;
                if (PowerSphere.IsEnergised)
                    TurnLightsOn();
            };
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

    private void TurnLightsOn()
    {
        areLightsOn = true;
        AudioController.Instance?.PlayGlobalSound("LightsOn");
    }
}
