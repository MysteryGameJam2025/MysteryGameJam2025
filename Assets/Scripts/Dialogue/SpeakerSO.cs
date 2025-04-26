using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Speaker", menuName = "Dialogue/Speaker")]
public class SpeakerSO : ScriptableObject
{
    [SerializeField]
    private string prettyName;
    public string PrettyName => prettyName;
    [SerializeField]
    private Color color;
    private Color Color => color;

    public string ToTaggedString()
        => $"<color=#{Color.ToHexString()}>{PrettyName}: </color>";
}