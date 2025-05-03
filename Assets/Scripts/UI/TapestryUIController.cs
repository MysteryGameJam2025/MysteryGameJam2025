using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapestryUIController : MonoBehaviour
{
    [SerializeField]
    private Button closeButton;
    public Button CloseButton => closeButton;

    [SerializeField]
    private CanvasGroup group;
    public CanvasGroup Group => group;

    [SerializeField]
    private Image[] images;
    private Image[] Images => images;

    public void SetSprites(List<Symbol> symbols)
    {
        for(int i = 0; i < symbols.Count; i++)
        {
            Images[i].sprite = symbols[i].SymbolSprite;
        }
    }
}
