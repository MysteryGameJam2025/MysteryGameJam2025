using FriedSynapse.FlowEnt;
using UnityEngine;

public class FourthRoomController : MonoBehaviour
{
    [SerializeField]
    private Material endSequence;
    private Material EndSequence => endSequence;
    [SerializeField]
    private DialogueSectionSO rhoDialogue;
    private DialogueSectionSO RhoDialogue => rhoDialogue;
    [SerializeField]
    private DialogueSectionSO terimalDialogue;
    private DialogueSectionSO TerimalDialogue => terimalDialogue;

    private AbstractAnimation FinalSequenceAnimation { get; set; }

    private const string ProgressRef = "_Progress";
    private const string WipeX = "_WipeX";
    private const string WipeY = "_WipeY";


    public void OnFinalNotePickup()
    {
        GauntletController.Instance.UIController.Hide();
        DialogueSingleton.Instance.OnSectionCompleted = FinalSequence;
        DialogueSingleton.Instance.EnqueueDialogue(RhoDialogue);
    }

    void FinalSequence()
    {
        PlayerController.Instance.LockControls();
        FinalSequenceAnimation?.Stop();
        GlitchManager.Instance.PlaySlowBuildup();
        FinalSequenceAnimation = new Flow()
            .QueueDelay(3)
            .Queue(new Tween(4)
                .For(EndSequence)
                .FloatTo(ProgressRef, 0, 1))
            .QueueDelay(2)
            .Queue(new Tween(0.5f)
                .For(EndSequence)
                .FloatTo(WipeY, 1))
            .Queue(new Tween(0.25f)
                .For(EndSequence)
                .FloatTo(WipeX, 1))
            .QueueDelay(5)
            .OnCompleted(() =>
            {
                FullScreenDialogueSingleton.Instance.OnSectionCompleted = ReturnToMenu;
                FullScreenDialogueSingleton.Instance.EnqueueDialogue(TerimalDialogue);
            })
            .Start();
    }

    void ReturnToMenu()
    {
        GlitchManager.Instance.StopGlitches();
        EndSequence.SetFloat(WipeX, 0);
        EndSequence.SetFloat(WipeY, 0);
        EndSequence.SetFloat(ProgressRef, 0);
        SceneController.Instance.LoadScene("Credits");
    }
}
