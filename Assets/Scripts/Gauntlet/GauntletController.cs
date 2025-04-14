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
    [SerializeField]
    private GauntletVisuals gauntletVisuals;
    private GauntletVisuals GauntletVisuals => gauntletVisuals;
    private Transform self;
    private Transform Self => self ??= transform;
    private bool IsLockingInteraction { get; set; }

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
            ActivateInteractable();
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
        if (interactable != currentInteractable && interactable != null)
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
        if (currentInteractable == null)
        {
            return;
        }
        currentInteractable.OnInteractionHoverEnd();
    }

    private void ActivateInteractable()
    {
        if (IsLockingInteraction || !currentInteractable || previousInteractable == currentInteractable)
            return;


        if (currentInteractable is SymbolPlate)
        {
            IsLockingInteraction = true;
            SymbolPlate symbolPlate = currentInteractable as SymbolPlate;
            symbolPlate.HidePrompt();
            //NOTE: Play visual effect if the interactable is a symbol plate
            GauntletVisuals.PlayEffect(currentInteractable.transform, () =>
            {
                IsLockingInteraction = false;
                currentInteractable.OnInteract(new InteractionEvent()
                {
                    EquippedSymbol = CurrentSymbol
                });
            });
        }
        else
        {
            currentInteractable.OnInteract(new InteractionEvent()
            {
                EquippedSymbol = CurrentSymbol
            });
        }
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
