using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : MonoBehaviour
{
    private int symbolIndex = 0;

    [SerializeField]
    private GauntletUIController uiController;
    private GauntletUIController UIController => uiController;

    [SerializeField]
    private InputActionReference activate;
    private InputActionReference Activate => activate;

    [SerializeField]
    private InputActionReference next;
    private InputActionReference Next => next;

    [SerializeField]
    private InputActionReference previous;
    private InputActionReference Previous => previous;

    [SerializeField]
    private Symbol currentSymbol;
    private Symbol CurrentSymbol { get => currentSymbol; set => currentSymbol = value; }

    [SerializeField]
    private List<Symbol> availableSymbols;
    private List<Symbol> AvailableSymbols => availableSymbols;

    public Action<SymbolActivatableBase> OnActivation;

    private SymbolActivatableBase currentActivatable;

    private SymbolActivatableBase previousActivatable;
    public SymbolActivatableBase PreviousActivatable => previousActivatable;

    private void Start()
    {
        UIController?.SetSymbolInUI(CurrentSymbol);
    }

    // Update is called once per frame
    void Update()
    {
        if (Next.action.WasPressedThisFrame())
            ChangeSymbol(++symbolIndex);

        if (Previous.action.WasPressedThisFrame())
            ChangeSymbol(--symbolIndex);

        if (Activate.action.WasPressedThisFrame())
            ActivateSymbol();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SymbolActivatableBase activatable))
        {
            currentActivatable = activatable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SymbolActivatableBase activatable))
        {
            currentActivatable = null;
        }
    }

    private void ActivateSymbol()
    {
        if (!currentActivatable || previousActivatable == currentActivatable)
            return;

        OnActivation?.Invoke(currentActivatable);
        currentActivatable?.OnSymbolInteract(CurrentSymbol, this);
        previousActivatable = currentActivatable;
    }

    private void ChangeSymbol(int symbolIndex)
    {
        symbolIndex = (symbolIndex + AvailableSymbols.Count) % AvailableSymbols.Count;
        CurrentSymbol = AvailableSymbols[symbolIndex];

        UIController?.SetSymbolInUI(CurrentSymbol);
        PreviousActivatable?.DropInteraction();
        previousActivatable = null;
        OnActivation = null;
    }
}
