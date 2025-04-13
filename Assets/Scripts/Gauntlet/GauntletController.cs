using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

    [SerializeField]
    private Transform interactablesHandle;
    private Transform InteractablesHandle => interactablesHandle;
    [SerializeField]
    private LayerMask interactablesLayerMask;
    private LayerMask InteractablesLayerMask => interactablesLayerMask;
    private Transform self;
    private Transform Self => self ??= transform;

    public Action<AbstractInteractable> OnActivation;

    private AbstractInteractable currentInteractable;

    private AbstractInteractable previousInteractable;
    public AbstractInteractable PreviousActivatable => previousInteractable;

    private const float InteractablesRange = 3f;


    private void Start()
    {
        UIController?.SetSymbolInUI(CurrentSymbol);
    }

    void Update()
    {
        if (Next.action.WasPressedThisFrame())
            ChangeSymbol(++symbolIndex);

        if (Previous.action.WasPressedThisFrame())
            ChangeSymbol(--symbolIndex);

        if (Activate.action.WasPressedThisFrame())
            ActivateSymbol();
    }

    void FixedUpdate()
    {
        InteractablesCollisions();
    }

    void InteractablesCollisions()
    {
        Collider[] interactables = Physics.OverlapSphere(InteractablesHandle.position, InteractablesRange, InteractablesLayerMask);

        if (interactables.Length == 0)
        {
            EndHover();
            currentInteractable = null;
            return;
        }

        Collider closestInteractable = interactables.OrderBy(interactable => Vector3.Distance(interactable.transform.position, Self.position)).First();
        AbstractInteractable interactable = closestInteractable.gameObject.GetComponent<AbstractInteractable>();
        if (interactable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                previousInteractable = currentInteractable;
                EndHover();
            }
            interactable.OnInteractionHoverStart();
            currentInteractable = interactable;
        }

    }

    void EndHover()
    {
        currentInteractable.OnInteractionHoverEnd();
    }

    private void ActivateSymbol()
    {

        if (!currentInteractable || previousInteractable == currentInteractable)
            return;

        currentInteractable.OnInteract();

        // OnActivation?.Invoke(currentActivatable);
        // currentActivatable?.OnSymbolInteract(CurrentSymbol, this);
        // previousActivatable = currentActivatable;
    }

    private void ChangeSymbol(int symbolIndex)
    {
        symbolIndex = (symbolIndex + AvailableSymbols.Count) % AvailableSymbols.Count;
        CurrentSymbol = AvailableSymbols[symbolIndex];

        UIController?.SetSymbolInUI(CurrentSymbol);
        //PreviousActivatable?.DropInteraction();
        previousInteractable = null;
        OnActivation = null;
    }
}
