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

    [SerializeField]
    private Symbol attraction;
    private Symbol Attraction => attraction;
    [SerializeField]
    private Symbol connection;
    private Symbol Connection => connection;
    [SerializeField]
    private GameObject attractionEffect;
    private GameObject AttractionEffect => attractionEffect;

    public void SetTarget(SymbolActivatableBase target)
    {
        currentTarget = target;
        if (currentSymbol == Attraction)
        {
            shouldMoveTowardsTarget = true;
            AttractionEffect.SetActive(true);
            targetTransform = currentTarget.transform;
        }

        if (currentSymbol == Connection)
        {
            if (target.TryGetComponent(out DoorControl doorControl))
            {
                connectedControlPanel = doorControl;
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentTarget != null)
        {
            if (shouldMoveTowardsTarget)
            {

                if (Vector3.Distance(targetTransform.position, transform.position) <= StoppingDistance)
                {
                    AttractionEffect.SetActive(false);
                    shouldMoveTowardsTarget = false;
                    currentTarget = null;
                    RB.isKinematic = true;
                }
                RB.AddForce((targetTransform.position - transform.position).normalized * Speed);
            }
        }
    }
}
