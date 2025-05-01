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
    private Symbol harmony;
    private Symbol Harmony => harmony;
    [SerializeField]
    private Symbol learning;
    private Symbol Learning => learning;

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
    private bool IsFullyRepaired { get; set; }
    private bool IsFirstDevicePlayingCorrectMelody { get; set; }
    private bool IsSecondDevicePlayingCorrectMelody { get; set; }

    private AbstractAnimation LeftTrumpetRepairAnimation { get; set; }
    private AbstractAnimation RightTrumpetRepairAnimation { get; set; }

    private Symbol FirstMusicDeviceActiveSymbol { get; set; }
    private Symbol SecondMusicDeviceActiveSymbol { get; set; }

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
        LeftTrumpet.gameObject.SetActive(true);
        CheckState();
    }

    public void PlaceRightTrumpet()
    {
        HasRightTrumpetBeenPlaced = true;
        RightTrumpet.gameObject.SetActive(true);
        CheckState();
    }

    public void FirstMusicDeviceSymbolPlateUsed(Symbol symbol)
    {
        FirstMusicDeviceActiveSymbol = symbol;
        CheckState();
    }

    public void SecondMusicDeviceSymbolPlateUsed(Symbol symbol)
    {
        SecondMusicDeviceActiveSymbol = symbol;
        CheckState();
    }

    void CheckState()
    {
        CheckForTrumpetRepair();
        CheckForFirstDeviceMelodyPlaying();
        CheckForCorrectMelodies();
    }

    void CheckForTrumpetRepair()
    {
        if (FirstMusicDeviceActiveSymbol == Togetherness)
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

    void CheckForFirstDeviceMelodyPlaying()
    {
        if (FirstMusicDeviceActiveSymbol == Harmony && IsFullyRepaired)
        {
            StartFirstDeviceMelody();
        }
        else
        {
            StopFirstDeviceMelody();
        }
    }

    void CheckForCorrectMelodies()
    {
        if (SecondMusicDeviceActiveSymbol == Harmony)
        {
            PlaySecondDeviceIncorrectMelody();
            return;
        }

        if (SecondMusicDeviceActiveSymbol == Learning && IsFullyRepaired && FirstMusicDeviceActiveSymbol == Harmony)
        {
            PlaySecondDeviceCorrectMelody();
            return;
        }

        StopSecondDeviceMelody();
    }



    [EasyButtons.Button]
    void PlayLeftTrumpetRepairAnimation()
    {
        LeftTrumpetRepairAnimation?.Stop();
        LeftTrumpetRepairAnimation = new Tween(3.1f)
            .For(LeftTrumpet)
            .MoveTo(LeftTrumpetEnd.position)
            .RotateTo(LeftTrumpetEnd.rotation)
            .SetEasing(Easing.EaseInOutSine)
            .OnCompleted(() =>
            {
                if (HasRightTrumpetBeenRepaired)
                {
                    IsFullyRepaired = true;
                    CheckState();
                }
            })
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
            .OnCompleted(() =>
            {
                if (HasLeftTrumpetBeenRepaired)
                {
                    IsFullyRepaired = true;
                    CheckState();
                }
            })
            .Start();
    }


    void StartFirstDeviceMelody()
    {
        IsFirstDevicePlayingCorrectMelody = true;
        Debug.Log("Toot toot!");
    }

    void StopFirstDeviceMelody()
    {
        IsFirstDevicePlayingCorrectMelody = false;
        Debug.Log("Jazz music stops");
    }

    void PlaySecondDeviceCorrectMelody()
    {
        Debug.Log("You win!");
    }

    void PlaySecondDeviceIncorrectMelody()
    {
        IsSecondDevicePlayingCorrectMelody = false;
        Debug.Log("BRRRRRRRRR");
    }

    void StopSecondDeviceMelody()
    {
        IsSecondDevicePlayingCorrectMelody = false;
        Debug.Log("Rock music stops");
    }
}
