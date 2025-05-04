using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    [SerializeField]
    private DialogueSectionSO blackScreenDialogue;
    private DialogueSectionSO BlackScreenDialogue => blackScreenDialogue;
    [SerializeField]
    private DialogueSectionSO inLevelDialogue;
    private DialogueSectionSO InLevelDialogue => inLevelDialogue;
    [SerializeField]
    private DialogueSectionSO afterNoteDialogue;
    private DialogueSectionSO AfterNoteDialogue => afterNoteDialogue;
    [SerializeField]
    private DoorControl caveDoor;
    private DoorControl CaveDoor => caveDoor;


    [Header("Debug")]
    [SerializeField]
    private bool playIntro;
    private bool PlayIntro => playIntro;

    void Awake()
    {
        if (PlayIntro)
        {
            FullScreenDialogueSingleton.Instance.OnSectionCompleted = StartInLevelDialogue;
            FullScreenDialogueSingleton.Instance.EnqueueDialogue(BlackScreenDialogue);
        }
        else
        {
            FullScreenDialogueSingleton.Instance.Hide();
            CaveDoor.Open();
        }
    }

    void StartInLevelDialogue()
    {
        DialogueSingleton.Instance.OnSectionCompleted = OpenNote;
        DialogueSingleton.Instance.EnqueueDialogue(InLevelDialogue);
    }

    void OpenNote()
    {
        OverarchingNoteController.Instance?.ShowNote(onNoteClose: StartAfterNoteDialogue);
    }

    void StartAfterNoteDialogue()
    {
        DialogueSingleton.Instance.OnSectionCompleted = OpenStartDoor;
        DialogueSingleton.Instance.EnqueueDialogue(AfterNoteDialogue);
    }

    void OpenStartDoor()
    {
        CaveDoor.Open();
    }
}
