using System;
using UnityEngine;

public abstract class SymbolActivatableBase : MonoBehaviour
{
    protected Symbol currentSymbol;
    public Symbol CurrentSymbol => currentSymbol;

    public virtual void SetCurrentSymbol(Symbol symbol)
    {
        currentSymbol = symbol;
    }
}
