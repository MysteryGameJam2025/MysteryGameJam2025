using UnityEngine;

[System.Serializable]
public class DialogueData
{
    [SerializeField]
    private SpeakerSO speaker;
    public SpeakerSO Speaker => speaker;
    [SerializeField]
    [TextArea]
    private string dialogue;
    public string Dialogue => dialogue;

    public string ToTaggedString()
    {
        if (Speaker == null)
        {
            return Dialogue;
        }

        return $"{Speaker.ToTaggedString()}{Dialogue}";
    }
}