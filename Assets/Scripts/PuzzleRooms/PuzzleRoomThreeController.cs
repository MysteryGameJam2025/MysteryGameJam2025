using UnityEngine;

public class PuzzleRoomThreeController : MonoBehaviour
{
    [SerializeField]
    private Symbol ruin;
    private Symbol Ruin => ruin;
    [SerializeField]
    private Symbol fulfill;
    private Symbol Fulfill => fulfill;
    [SerializeField]
    private Symbol entrust;
    private Symbol Entrust => entrust;
    [SerializeField]
    private StairsController stairs;
    private StairsController Stairs => stairs;
    [SerializeField]
    private CrystalController crystal;
    private CrystalController Crystal => crystal;
    [SerializeField]
    private GameObject bottomCrystal;
    private GameObject BottomCrystal => bottomCrystal;
    [SerializeField]
    private GameObject holoCrystal;
    private GameObject HoloCrystal => holoCrystal;
    [SerializeField]
    private GameObject brokenTransmitter;
    private GameObject BrokenTransmitter => brokenTransmitter;
    [SerializeField]
    private GameObject topTransmitter;
    private GameObject TopTransmitter => topTransmitter;
    [SerializeField]
    private DoorControl exitDoor;
    private DoorControl ExitDoor => exitDoor;
    [SerializeField]
    private GameObject beamVfx;
    private GameObject BeamVfx => beamVfx;
    [SerializeField]
    private GameObject fixingPrompt;
    private GameObject FixingPrompt => fixingPrompt;

    [SerializeField]
    private MessageData thirdNote;
    private MessageData ThirdNote => thirdNote;

    [Header("Dialogue")]
    [SerializeField]
    private DialogueSectionSO shoeInteractionDialogue;
    private DialogueSectionSO ShoeInteractionDialogue => shoeInteractionDialogue;

    [SerializeField]
    private DialogueSectionSO playerAttempsExitDialogue;
    private DialogueSectionSO PlayerAttempsExitDialogue => playerAttempsExitDialogue;

    [SerializeField]
    private DialogueSectionSO puzzleCompletionDialogue;
    private DialogueSectionSO PuzzleCompletionDialogue => puzzleCompletionDialogue;

    [SerializeField]
    private DialogueSectionSO postThirdTranslatedNoteDialogue;
    private DialogueSectionSO PostThirdTranslatedNoteDialogue => postThirdTranslatedNoteDialogue;

    private bool IsStairsDestroyed { get; set; }
    private bool IsBrokenEmitterFixed { get; set; }
    private bool IsCrystalDropped { get; set; }
    private bool IsHoloCrystalActive { get; set; }

    public void OnStairsSymbolPlateUsed(Symbol symbol)
    {
        if (!IsStairsDestroyed && symbol == Ruin)
        {
            Stairs.DestroyRealStairs();
            IsStairsDestroyed = true;
        }

        if (IsStairsDestroyed && symbol == Fulfill)
        {
            Stairs.SetHologramStarisEnabled(true);
        }
        else
        {
            Stairs.SetHologramStarisEnabled(false);
        }
    }

    public void OnEmitterSymbolPlateUsed(Symbol symbol)
    {
        if (!IsCrystalDropped && symbol == Ruin)
        {
            IsCrystalDropped = true;
            TopTransmitter.SetActive(false);
            BrokenTransmitter.SetActive(true);
            // TODO: Add vfx
            Crystal.DropCrystal();
            FixingPrompt.SetActive(true);
        }

        if (IsCrystalDropped && symbol == Fulfill)
        {
            IsHoloCrystalActive = true;
            HoloCrystal.SetActive(true);
        }

        if (IsBrokenEmitterFixed && IsHoloCrystalActive && symbol == Entrust)
        {
            BeamVfx.SetActive(true);
            OnPuzzleCompleted();
        }
    }

    public void OnEmitterFixed()
    {
        IsBrokenEmitterFixed = true;
        Crystal.gameObject.SetActive(false);
        BottomCrystal.SetActive(true);
        FixingPrompt.SetActive(false);
    }

    public void OnPuzzleCompleted()
    {
        ExitDoor.Open();

        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PuzzleCompletionDialogue);
    }

    public void PlayerFindsShoe()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(ShoeInteractionDialogue);
    }

    public void PlayerAttemptsExit()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PlayerAttempsExitDialogue);
    }

    public void PickUpNote()
    {
        MessageDecoderController.Instance?.OpenMessage(ThirdNote, AfterNote);
    }

    private void AfterNote()
    {
        DialogueSingleton.Instance.OnSectionCompleted = null;
        DialogueSingleton.Instance.EnqueueDialogue(PostThirdTranslatedNoteDialogue);
    }
}
