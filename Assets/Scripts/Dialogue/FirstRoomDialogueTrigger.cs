using UnityEngine;

public class FirstRoomDialogueTrigger : MonoBehaviour
{
    private bool hasBeenTriggered;

    [SerializeField]
    private DialogueSectionSO roomOneDialogue;
    private DialogueSectionSO RoomOneDialogue => roomOneDialogue;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            DialogueSingleton.Instance.OnSectionCompleted = null;
            DialogueSingleton.Instance.EnqueueDialogue(RoomOneDialogue);
        }
    }
}
