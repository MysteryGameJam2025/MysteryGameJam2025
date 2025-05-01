using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    [SerializeField]
    private DialogueSectionSO blackScreenDialogue;
    private DialogueSectionSO BlackScreenDialogue => blackScreenDialogue;
    [SerializeField]
    private DialogueSectionSO inLevelDialogue;
    private DialogueSectionSO InLevelDialogue => inLevelDialogue;


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
        }
    }

    void StartInLevelDialogue()
    {
        DialogueSingleton.Instance.OnSectionCompleted = OpenNote;
        DialogueSingleton.Instance.EnqueueDialogue(InLevelDialogue);
    }

    void OpenNote()
    {

    }
}
