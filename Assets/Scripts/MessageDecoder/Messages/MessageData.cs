using UnityEngine;

[CreateAssetMenu]
public class MessageData : ScriptableObject
{
    [SerializeField]
    private TextAsset message;
    public TextAsset Message => message;

    [SerializeField]
    private TextAsset translatedMessage;
    public TextAsset TranslatedMessage => translatedMessage;

    [SerializeField]
    private DialogueSectionSO postSolveDialog;
    public DialogueSectionSO PostSolveDialog => postSolveDialog;

    [SerializeField]
    private DialogueSectionSO preSolveDialog;
    public DialogueSectionSO PreSolveDialog => preSolveDialog;

    [SerializeField]
    private Symbol[] symbolsInMessage;
    public Symbol[] SymbolsInMessage => symbolsInMessage;

    [SerializeField]
    private string[] alternatePossibilites;
    public string[] AlternatePossibilites => alternatePossibilites;
}
