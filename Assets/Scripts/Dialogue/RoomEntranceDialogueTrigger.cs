using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.EventSystems.EventTrigger;

public class RoomEntranceDialogueTrigger : MonoBehaviour
{
    private bool hasBeenTriggered;

    [SerializeField]
    private DialogueSectionSO entranceDialogue;
    private DialogueSectionSO EntranceDialogue => entranceDialogue;

    [SerializeField]
    private UnityEvent onEntry;
    private UnityEvent OnEntry => onEntry;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered)
        {
            hasBeenTriggered = true;
            OnEntry?.Invoke();
            if (EntranceDialogue != null)
            {
                DialogueSingleton.Instance.OnSectionCompleted = null;
                DialogueSingleton.Instance.EnqueueDialogue(EntranceDialogue);
            }
        }
    }
}
