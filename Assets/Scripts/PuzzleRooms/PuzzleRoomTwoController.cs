using FriedSynapse.FlowEnt;
using UnityEngine;

public class PuzzleRoomTwoController : MonoBehaviour
{
    [SerializeField]
    private GameObject leftTrumpetPlacement;
    private GameObject LeftTrumpetPlacement => leftTrumpetPlacement;
    [SerializeField]
    private GameObject rightTrumpetPlacement;
    private GameObject RightTrumpetPlacement => rightTrumpetPlacement;
    [SerializeField]
    private Symbol togetherness;
    private Symbol Togetherness => togetherness;

    [SerializeField]
    private Transform leftTrumpet;
    private Transform LeftTrumpet => leftTrumpet;
    [SerializeField]
    private Transform rightTrumpet;
    private Transform RightTrumpet => rightTrumpet;
    [SerializeField]
    private Transform leftTrumpetEnd;
    private Transform LeftTrumpetEnd => leftTrumpetEnd;
    [SerializeField]
    private Transform rightTrumpetEnd;
    private Transform RightTrumpetEnd => rightTrumpetEnd;

    private bool HasLeftTrumpetBeenPlaced { get; set; }
    private bool HasRightTrumpetBeenPlaced { get; set; }
    private bool HasLeftTrumpetBeenRepaired { get; set; }
    private bool HasRightTrumpetBeenRepaired { get; set; }

    private AbstractAnimation LeftTrumpetRepairAnimation { get; set; }
    private AbstractAnimation RightTrumpetRepairAnimation { get; set; }

    private Symbol MusicDeviceActiveSymbol { get; set; }

    void Awake()
    {
        HasLeftTrumpetBeenPlaced = false;
        HasRightTrumpetBeenPlaced = false;
    }

    public void PickupLeftTrumpet()
    {
        LeftTrumpetPlacement.SetActive(true);
    }

    public void PickupRightTrumpet()
    {
        RightTrumpetPlacement.SetActive(true);
    }

    public void PlaceLeftTrumpet()
    {
        HasLeftTrumpetBeenPlaced = true;
        LeftTrumpet.parent.gameObject.SetActive(true);
        CheckForTrumpetRepair();
    }

    public void PlaceRightTrumpet()
    {
        HasRightTrumpetBeenPlaced = true;
        RightTrumpet.parent.gameObject.SetActive(true);
        CheckForTrumpetRepair();
    }

    public void MusicDeviceSymbolPlateUsed(Symbol symbol)
    {
        MusicDeviceActiveSymbol = symbol;
        CheckForTrumpetRepair();
    }

    void CheckForTrumpetRepair()
    {
        if (MusicDeviceActiveSymbol == Togetherness)
        {
            if (HasLeftTrumpetBeenPlaced && !HasLeftTrumpetBeenRepaired)
            {
                HasLeftTrumpetBeenRepaired = true;
                PlayLeftTrumpetRepairAnimation();
            }

            if (HasRightTrumpetBeenPlaced && !HasRightTrumpetBeenRepaired)
            {
                HasRightTrumpetBeenRepaired = true;
                PlayRightTrumpetRepairAnimation();
            }
        }
    }

    [EasyButtons.Button]
    void PlayLeftTrumpetRepairAnimation()
    {
        LeftTrumpetRepairAnimation?.Stop();
        LeftTrumpetRepairAnimation = new Tween(3)
            .For(LeftTrumpet)
            .MoveTo(LeftTrumpetEnd.position)
            .RotateTo(LeftTrumpetEnd.rotation)
            .SetEasing(Easing.EaseInOutSine)
            .Start();
    }

    [EasyButtons.Button]
    void PlayRightTrumpetRepairAnimation()
    {
        RightTrumpetRepairAnimation?.Stop();
        RightTrumpetRepairAnimation = new Tween(3)
            .For(RightTrumpet)
            .MoveTo(RightTrumpetEnd.position)
            .RotateTo(RightTrumpetEnd.rotation)
            .SetEasing(Easing.EaseInOutSine)
            .Start();
    }



}
