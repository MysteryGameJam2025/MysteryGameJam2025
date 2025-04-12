using System;
using UnityEngine;

public abstract class SymbolActivatableBase : MonoBehaviour
{
    protected Symbol currentSymbol;
    public Symbol CurrentSymbol => currentSymbol;

    public virtual void OnSymbolInteract(Symbol symbol, GauntletController gauntlet)
    {
        currentSymbol = symbol;
    }

    public virtual void DropInteraction()
    {
        currentSymbol = null;
    }
}
