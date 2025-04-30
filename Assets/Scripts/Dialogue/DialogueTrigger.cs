using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]
    private DialogueSectionSO dialogue;
    private DialogueSectionSO Dialogue => dialogue;
    void OnTriggerEnter(Collider other)
    {
        DialogueSingleton.Instance.EnqueueDialogue(Dialogue);
        Destroy(gameObject);
    }
}