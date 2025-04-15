using NUnit.Framework;
using UnityEngine;



[CreateAssetMenu]
public class Symbol : ScriptableObject
{
    [SerializeField]
    private Sprite symbolSprite;
    public Sprite SymbolSprite => symbolSprite;

    [SerializeField]
    private string symbolName;
    public string SymbolName => symbolName;

}
