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
    private Symbol energy;
    private Symbol Energy => energy;
    [SerializeField]
    private GameObject attractionEffect;
    private GameObject AttractionEffect => attractionEffect;
    [SerializeField]
    private Material energizedMaterial;
    private Material EnergizedMaterial => energizedMaterial;
    [SerializeField]
    private GameObject energizedEffect;
    private GameObject EnergizedEffect => energizedEffect;

    private MeshRenderer meshRenderer;
    private MeshRenderer MeshRenderer => meshRenderer ??= GetComponent<MeshRenderer>();

    private AudioSource RollingSource { get; set; }

    public void SetTarget(SymbolActivatableBase target)
    {
        currentTarget = target;
        if (currentSymbol == Attraction)
        {
            shouldMoveTowardsTarget = true;
            AttractionEffect.SetActive(true);
            targetTransform = currentTarget.transform;
            RollingSource = AudioController.Instance.PlayLocalSound("BallRolling", gameObject, shouldPlay: false);
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
                AudioController.Instance.PlayLocalSound("BallRolling", gameObject, false);
                if (Vector3.Distance(targetTransform.position, transform.position) <= StoppingDistance)
                {
                    AttractionEffect.SetActive(false);
                    shouldMoveTowardsTarget = false;
                    currentTarget = null;
                    RB.isKinematic = true;
                    RollingSource?.Stop();
                    AudioController.Instance.PlayLocalSound("BallClick", gameObject);
                }
                RB.AddForce((targetTransform.position - transform.position).normalized * Speed);
            }
        }
    }

    public override void SetCurrentSymbol(Symbol symbol)
    {
        base.SetCurrentSymbol(symbol);

        if (symbol == Energy)
        {
            MeshRenderer.materials = new Material[2] { MeshRenderer.materials[0], EnergizedMaterial };
            EnergizedEffect.SetActive(true);
        }
        else
        {
            MeshRenderer.materials = new Material[1] { MeshRenderer.materials[0] };
            EnergizedEffect.SetActive(false);
        }
    }
}
