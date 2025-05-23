using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class GauntletController : AbstractMonoBehaviourSingleton<GauntletController>
{
    private int symbolIndex = 0;
    private int SymbolIndex { get => symbolIndex; set => symbolIndex = value; }

    [SerializeField]
    private GauntletUIController uiController;
    public GauntletUIController UIController => uiController;

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
    [SerializeField]
    private Animator playerAnimator;
    private Animator PlayerAnimator => playerAnimator;
    [SerializeField]
    private PlayerController player;
    private PlayerController Player => player;
    private Transform self;
    private Transform Self => self ??= transform;
    private bool IsLockingInteraction { get; set; }

    public Action<AbstractInteractable> OnActivation;

    private AbstractInteractable currentInteractable;

    private AbstractInteractable previousInteractable;
    public AbstractInteractable PreviousActivatable => previousInteractable;

    private const float InteractablesRange = 2f;
    private int UseGauntletHash { get; set; }
    private int IsPickupHash { get; set; }

    private void Start()
    {
        IsPickupHash = Animator.StringToHash("IsPickup");
        UseGauntletHash = Animator.StringToHash("UseGauntlet");
    }

    void Update()
    {
        if (PlayerController.Instance.IsControlsLocked)
        {
            return;
        }

        if (!UIController.IsAnimationPlaying && !IsLockingInteraction)
        {
            if (Next.action.WasPressedThisFrame())
                ChangeSymbol(++SymbolIndex, true);

            if (Previous.action.WasPressedThisFrame())
                ChangeSymbol(--SymbolIndex, false);
        }


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

        Collider closestInteractable = interactables.OrderBy(interactable => Vector3.Distance(interactable.ClosestPoint(Self.position), Self.position)).First();
        AbstractInteractable interactable = closestInteractable.gameObject.GetComponent<AbstractInteractable>();
        if (interactable != currentInteractable && interactable != null)
        {
            if (currentInteractable != null)
            {
                EndHover();
            }

            if(AvailableSymbols.Count == 0 && interactable is SymbolPlate)
            {
                return;
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

    public void OnGauntletFire()
    {
        Player.UnlockControls();
        PlayerAnimator.SetBool(UseGauntletHash, false);
        AbstractInteractable interactableToActivate = currentInteractable;
        GauntletVisuals.PlayEffect(interactableToActivate.transform, () =>
        {
            IsLockingInteraction = false;
            interactableToActivate.OnInteract(new InteractionEvent()
            {
                EquippedSymbol = CurrentSymbol
            });
        });
    }

    public void OnPickup()
    {
        PlayerAnimator.SetBool(IsPickupHash, false);
        currentInteractable.OnInteract(new InteractionEvent()
        {
            EquippedSymbol = CurrentSymbol
        });
    }

    public void OnPickupCompleted()
    {
        Player.UnlockControls();
    }

    private void ActivateInteractable()
    {
        if (IsLockingInteraction || !currentInteractable || previousInteractable == currentInteractable)
            return;


        if (currentInteractable is SymbolPlate)
        {
            Player.LockControls();
            PlayerAnimator.SetBool(UseGauntletHash, true);
            IsLockingInteraction = true;
            SymbolPlate symbolPlate = currentInteractable as SymbolPlate;
            symbolPlate.HidePrompt();
            //NOTE: Play visual effect if the interactable is a symbol plate
        }
        else if (currentInteractable is Pickup)
        {
            Player.LockControls();
            PlayerAnimator.SetBool(IsPickupHash, true);

            AudioController.Instance?.PlayLocalSound("PickUp", gameObject);
        }
        else
        {
            currentInteractable.OnInteract(new InteractionEvent()
            {
                EquippedSymbol = CurrentSymbol
            });
        }
        previousInteractable = currentInteractable;
    }

    private void ChangeSymbol(int symbolIndex, bool isNext)
    {
        SymbolIndex = (symbolIndex + AvailableSymbols.Count) % AvailableSymbols.Count;
        CurrentSymbol = AvailableSymbols[SymbolIndex];

        UIController?.SetSymbolInUI(CurrentSymbol, isNext);
        //PreviousActivatable?.DropInteraction();
        previousInteractable = null;
        OnActivation = null;

        AudioController.Instance?.PlayLocalSound("SwapSymbol", gameObject);
    }

    public void SetSymbols(List<Symbol> symbolsToEquip)
    {
        bool isFirstSymbols = availableSymbols.Count == 0;

        availableSymbols = symbolsToEquip;
        currentSymbol = availableSymbols[0];
        symbolIndex = 0;

        UIController.SetInitialSymbol(CurrentSymbol);

        if (isFirstSymbols)
        {
            UIController.Show();
        }
    }
}
