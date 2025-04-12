using System;
using UnityEngine;

public abstract class SymbolActivatableBase : MonoBehaviour
{
    protected Symbol currentSymbol;
    public Symbol CurrentSymbol => currentSymbol;

    public abstract void OnSymbolInteract(Symbol symbol, GauntletController gauntlet);

    public virtual void DropInteraction()
    {
        currentSymbol = null;
    }
}
