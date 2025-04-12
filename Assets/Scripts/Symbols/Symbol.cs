using NUnit.Framework;
using UnityEngine;

public enum SymbolType
{
    Attraction,
    Connection,
    Energy,
    Totality,
    Harmony,
    Substance,
    Deactivate,
    Rest,
    Expire
}

[CreateAssetMenu]
public class Symbol : ScriptableObject
{
    [SerializeField]
    private Sprite symbolSprite;
    public Sprite SymbolSprite => symbolSprite;

    [SerializeField]
    private string symbolName;
    public string SymbolName => symbolName;

    [SerializeField]
    private SymbolType symbolType;
    public SymbolType SymbolType => symbolType;
}
