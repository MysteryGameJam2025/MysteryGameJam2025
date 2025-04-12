using UnityEngine;

public class BackstopPuzzleComponent : SymbolActivatableBase
{
    public override void OnSymbolInteract(Symbol symbol, GauntletController gauntlet)
    {
        currentSymbol = symbol;
    }
}
