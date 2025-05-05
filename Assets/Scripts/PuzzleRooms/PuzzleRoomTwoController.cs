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
    [SerializeField]
    private DoorControl exitDoor;
    private DoorControl ExitDoor => exitDoor;
    [SerializeField]
    private GameObject trumpetVfx;
    private GameObject TrumpetVfx => trumpetVfx;
    [SerializeField]
    private GameObject harpVfx;
    private GameObject HarpVfx => harpVfx;
    [SerializeField]
    private GameObject badHarpVfx;
    private GameObject BadHarpVfx => badHarpVfx;

    [SerializeField]
    private MessageData secondNote;
    private MessageData SecondNote => secondNote;

    [Header("Dialogue")]
    [SerializeField]
    private DialogueSectionSO tapestryInteractionDialouge;
    private DialogueSectionSO TapestryInteractionDialouge => tapestryInteractionDialouge;

    [SerializeField]
    private DialogueSectionSO brokenHornInteractionDialouge;
    private DialogueSectionSO BrokenHornInteractionDialouge => brokenHornInteractionDialouge;

    [SerializeField]
    private DialogueSectionSO firstHornInteractionDialouge;
    private DialogueSectionSO FirstHornInteractionDialouge => firstHornInteractionDialouge;

    [SerializeField]
    private DialogueSectionSO discordentMelodyDialogue;
    private DialogueSectionSO DiscordentMelodyDialogue => discordentMelodyDialogue;

    [SerializeField]
    private DialogueSectionSO distressInteractionDialouge;
    private DialogueSectionSO DistressInteractionDialouge => distressInteractionDialouge;

    [SerializeField]
    private DialogueSectionSO puzzleCompletionDialouge;
    private DialogueSectionSO PuzzleCompletionDialouge => puzzleCompletionDialouge;

    [SerializeField]
    private DialogueSectionSO postSecondTranslatedNoteDialog;
    private DialogueSectionSO PostSecondTranslatedNoteDialog => postSecondTranslatedNoteDialog;

    private bool HasATrumpet { get; set; }
    private bool HasLeftTrumpetBeenPlaced { get; set; }
    private bool HasRightTrumpetBeenPlaced { get; set; }
    private bool HasLeftTrumpetBeenRepaired { get; set; }
    private bool HasRightTrumpetBeenRepaired { get; set; }
    private bool IsFullyRepaired { get; set; }
    private bool IsFirstDevicePlayingCorrectMelody { get; set; }
    private bool IsSecondDevicePlayingCorrectMelody { get; set; }
    private bool HasPlayedIncorectMelody { get; set; }
    private bool IsPlayerInRoom { get; set; }

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
        CheckHornDialogue();
    }

    public void PickupRightTrumpet()
    {
        RightTrumpetPlacement.SetActive(true);
        CheckHornDialogue();
    }

    private void CheckHornDialogue()
    {
        if (!HasATrumpet)
        {
            HasATrumpet = true;
            DialogueSingleton.Instance.OnSectionCompleted = null;
            DialogueSingleton.Instance.EnqueueDialogue(FirstHornInteractionDialouge);
        }
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
        if(!HasLeftTrumpetBeenPlaced && !HasRightTrumpetBeenPlaced)
        {
            DialogueSingleton.Instance.OnSectionCompleted = null;
            DialogueSingleton.Instance.EnqueueDialogue(BrokenHornInteractionDialouge);
        }

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
            if (!HasPlayedIncorectMelody)
            {
                HasPlayedIncorectMelody = true;
                DialogueSingleton.Instance.OnSectionCompleted = null;
                DialogueSingleton.Instance.EnqueueDialogue(DiscordentMelodyDialogue);
            }
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
        TrumpetVfx.SetActive(true);
        AudioController.Instance.FadeInWorldHorns();
    }

    void StopFirstDeviceMelody()
    {
        IsFirstDevicePlayingCorrectMelody = false;
        TrumpetVfx.SetActive(false);
        AudioController.Instance.FadeOutWorldHorns();
    }

    void PlaySecondDeviceCorrectMelody()
    {
        HarpVfx.SetActive(true);
        BadHarpVfx.SetActive(false);

        AudioController.Instance.FadeInHarpCorrect();
        AudioController.Instance.FadeOutHarpIncorrect();
        OnPuzzleCompleted();
    }

    void PlaySecondDeviceIncorrectMelody()
    {
        IsSecondDevicePlayingCorrectMelody = false;
        HarpVfx.SetActive(false);
        BadHarpVfx.SetActive(true);
        AudioController.Instance.FadeInHarpIncorrect();
        AudioController.Instance.FadeOutHarpCorrect();
    }

    void StopSecondDeviceMelody()
    {
        IsSecondDevicePlayingCorrectMelody = false;
        HarpVfx.SetActive(false);
        BadHarpVfx.SetActive(false);

        AudioController.Instance.FadeOutHarpCorrect();
        AudioController.Instance.FadeOutHarpIncorrect();
    }

    public void PickUpNote()
    {
        MessageDecoderController.Instance?.OpenMessage(SecondNote, AfterNote);
    }

    private void AfterNote()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PostSecondTranslatedNoteDialog);
    }

    public void InteractedWithTapestry()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(TapestryInteractionDialouge);
    }

    public void InteractedWithDistressBeacon()
    {
        GlitchManager.Instance.PlayShortGlitch();

        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(DistressInteractionDialouge);
    }

    public void OnPuzzleCompleted()
    {
        ExitDoor.Open();

        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PuzzleCompletionDialouge);
    }

    public void OnPlayerEntersExits()
    {
        IsPlayerInRoom = !IsPlayerInRoom;

        if (IsPlayerInRoom)
        {
            AudioController.Instance.FadeInWorldTheme();
            if (IsFirstDevicePlayingCorrectMelody)
                AudioController.Instance.FadeInWorldHorns();
            if (IsSecondDevicePlayingCorrectMelody)
                AudioController.Instance.FadeInHarpCorrect();
            if (BadHarpVfx.activeInHierarchy)
                AudioController.Instance.FadeInHarpIncorrect();
        }
        else
        {
            AudioController.Instance.FadeOutWorldTheme();
        }
    }
}
