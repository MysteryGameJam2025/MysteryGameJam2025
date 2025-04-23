using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue Section", menuName = "Dialogue/Dialogue Section")]
public class DialogueSectionSO : ScriptableObject
{
    [SerializeField]
    private List<DialogueData> dialogueData;
    public List<DialogueData> DialogueData => dialogueData;
}
