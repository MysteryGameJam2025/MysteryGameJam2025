using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerSphereController : SymbolActivatableBase
{
    private SymbolActivatableBase currentTarget;
    private Transform targetTransform;

    private bool shouldMoveTowardsTarget = false;

    private DoorControl connectedControlPanel;

    [SerializeField]
    private Rigidbody rb;
    private Rigidbody RB => rb;

    [SerializeField]
    private float speed;
    private float Speed => speed;

    [SerializeField]
    private float stoppingDistance;
    private float StoppingDistance => stoppingDistance;

    public override void OnSymbolInteract(Symbol symbol, GauntletController gauntlet)
    {
        // base.OnSymbolInteract(symbol, gauntlet);
        // switch (symbol.SymbolType)
        // {
        //     case SymbolType.Attraction:
        //     case SymbolType.Connection:
        //         if (gauntlet.PreviousActivatable?.CurrentSymbol.SymbolType == symbol.SymbolType)
        //         {
        //             SetTarget(gauntlet.PreviousActivatable);
        //             return;
        //         }
        //         gauntlet.OnActivation += SetTarget;
        //         break;
        //     case SymbolType.Energy:
        //         if (connectedControlPanel != null)
        //         {
        //             connectedControlPanel.SetPowered(true);
        //         }
        //         break;
        //     default:
        //         break;
        // }
    }

    public void SetTarget(SymbolActivatableBase target)
    {
        currentTarget = target;
        switch (currentSymbol.SymbolType)
        {
            case SymbolType.Attraction:
                shouldMoveTowardsTarget = true;
                targetTransform = currentTarget.transform;
                break;
            case SymbolType.Connection:
                if (target.TryGetComponent(out DoorControl doorControl))
                {
                    connectedControlPanel = doorControl;
                }
                break;
            default:
                break;
        }
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            if (shouldMoveTowardsTarget)
            {
                if (Vector3.Distance(targetTransform.position, transform.position) <= StoppingDistance)
                {
                    shouldMoveTowardsTarget = false;
                    currentTarget = null;
                    RB.isKinematic = true;
                }
                RB.AddForce((targetTransform.position - transform.position).normalized * Speed);
            }
        }
    }
}
