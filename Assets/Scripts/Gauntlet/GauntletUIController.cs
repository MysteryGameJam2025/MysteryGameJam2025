using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class GauntletUIController : MonoBehaviour
{
    [SerializeField]
    private Image symbolUIImage;
    public Image SymbolUIImage => symbolUIImage;

    [SerializeField]
    private TMP_Text symbolUIText;
    private TMP_Text SymbolUIText => symbolUIText;

    public void SetSymbolInUI(Symbol symbol)
    {
        SymbolUIImage.sprite = symbol.SymbolSprite;
        SymbolUIText.text = symbol.SymbolName;
    }
}
