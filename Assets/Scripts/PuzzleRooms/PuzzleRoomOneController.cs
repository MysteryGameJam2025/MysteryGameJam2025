using System.Collections.Generic;
using System.Xml.Serialization;
using FriedSynapse.FlowEnt;
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
    [SerializeField]
    private Material powerUpMaterial;
    private Material PowerUpMaterial => powerUpMaterial;
    [SerializeField]
    [ColorUsage(false, true)]
    private Color emmissionStart;
    private Color EmmissionStart => emmissionStart;
    [SerializeField]
    [ColorUsage(false, true)]
    private Color emissionEnd;
    private Color EmissionEnd => emissionEnd;


    bool isSphereMoved;

    bool areControlsConnected;

    private AbstractAnimation MaterialAnimation { get; set; }

    void Awake()
    {
        PowerUpMaterial.SetColor("_EmissionColor", EmmissionStart);
    }


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
        if (isSphereMoved && PowerSphere.CurrentSymbol == Energy)
        {
            MaterialAnimation?.Stop();
            MaterialAnimation = new Tween(2)
                .For(PowerUpMaterial)
                .ColorTo("_EmissionColor", EmissionEnd)
                .SetEasing(Easing.EaseOutSine)
                .OnCompleted(() =>
                {

                })
                .Start();
        }

        if (PowerSphere.CurrentSymbol == Connection && DoorControls.CurrentSymbol == Connection)
        {
            areControlsConnected = true;
        }
        else if (PowerSphere.CurrentSymbol == Energy && DoorControls.CurrentSymbol == Energy && areControlsConnected)
        {

            AudioController.Instance?.PlayLocalSound("LightsOn", DoorControls.gameObject);
            DoorControls.Open();
        }
    }
}
