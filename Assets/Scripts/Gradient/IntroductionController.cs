using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    [SerializeField]
    private DialogueSectionSO blackScreenDialogue;
    private DialogueSectionSO BlackScreenDialogue => blackScreenDialogue;
    [SerializeField]
    private DialogueSectionSO inLevelDialogue;
    private DialogueSectionSO InLevelDialogue => inLevelDialogue;

    void Awake()
    {
        FullScreenDialogueSingleton.Instance.OnSectionCompleted = StartInLevelDialogue;
        FullScreenDialogueSingleton.Instance.EnqueueDialogue(BlackScreenDialogue);
    }

    void StartInLevelDialogue()
    {
        DialogueSingleton.Instance.EnqueueDialogue(InLevelDialogue);
    }
}
