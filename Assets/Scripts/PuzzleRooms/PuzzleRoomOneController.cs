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
    private Symbol openness;
    private Symbol Openness => openness;

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

    [SerializeField]
    private DialogueSectionSO interactingWithTapestry;
    private DialogueSectionSO InteractingWithTapestry => interactingWithTapestry;
    [SerializeField]
    private DialogueSectionSO interactingWithBlood;
    private DialogueSectionSO InteractingWithBlood => interactingWithBlood;
    [SerializeField]
    private DialogueSectionSO firstSymbolPlateDialogue;
    private DialogueSectionSO FirstSymbolPlateDialogue => firstSymbolPlateDialogue;
    [SerializeField]
    private DialogueSectionSO completedPuzzleDialogue;
    private DialogueSectionSO CompletedPuzzleDialogue => completedPuzzleDialogue;

    [SerializeField]
    private MessageData firstNote;
    private MessageData FirstNote => firstNote;

    [SerializeField]
    private DialogueSectionSO postFirstTranslatedNoteDialog;
    private DialogueSectionSO PostFirstTranslatedNoteDialog => postFirstTranslatedNoteDialog;


    bool isSphereAtBottom = true;
    bool isSphereAtTop;
    bool areControlsConnected;
    bool areLightsOn;
    bool hasUsedSymbol;

    private AbstractAnimation MaterialAnimation { get; set; }

    void Awake()
    {
        PowerUpMaterial.SetColor("_EmissionColor", EmmissionStart);
    }

    public void PickUpFirstNote()
    {
        MessageDecoderController.Instance?.OpenMessage(FirstNote, AfterFirstNote);
    }

    private void AfterFirstNote()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PostFirstTranslatedNoteDialog);
    }

    public void SetSymbolBottomOfHill(Symbol symbol)
    {
        CheckFirstUse(symbol);
        if (!isSphereAtBottom)
        {
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);
        CheckSphereAndBackstop();
    }

    public void SetSymbolTopOfHill(Symbol symbol)
    {
        CheckFirstUse(symbol);
        if (!isSphereAtTop)
        {
            Backstop.SetCurrentSymbol(symbol);
            CheckSphereAndBackstop();
            return;
        }

        PowerSphere.SetCurrentSymbol(symbol);

        if (PowerSphere.IsEnergised && !areLightsOn)
        {
            TurnLightsOn();

        }


        CheckSphereAndDoorControls();
    }

    public void SetDoorSymbol(Symbol symbol)
    {
        CheckFirstUse(symbol);
        DoorControls.SetCurrentSymbol(symbol);
        CheckSphereAndDoorControls();
    }

    private void CheckFirstUse(Symbol symbol)
    {
        if (!hasUsedSymbol && symbol == Attraction)
        {
            hasUsedSymbol = true;
            DialogueSingleton.Instance.OnSectionCompleted = null;
            DialogueSingleton.Instance.EnqueueDialogue(FirstSymbolPlateDialogue);
        }
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
        if (DoorControls.CurrentSymbol == Openness && areLightsOn)
        {
            OnPuzzleCompleted();
        }
    }

    private void TurnLightsOn()
    {
        AudioController.Instance?.PlayGlobalSound("LightsOn");
        MaterialAnimation?.Stop();
        MaterialAnimation = new Tween(2)
            .For(PowerUpMaterial)
            .ColorTo("_EmissionColor", EmissionEnd)
            .SetEasing(Easing.EaseOutSine)
            .OnCompleted(() =>
            {
                areLightsOn = true;
                CheckSphereAndDoorControls();
            })
            .Start();
    }

    public void InteractedWithTapestry()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(InteractingWithTapestry);
    }

    public void InteractedWithBlood()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(InteractingWithBlood);
    }

    public void OnPuzzleCompleted()
    {
        DoorControls.Open();

        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(CompletedPuzzleDialogue);
    }
}
