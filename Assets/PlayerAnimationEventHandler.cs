using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    [SerializeField]
    private UnityEvent gauntletFireCallback;
    private UnityEvent GauntletFireCallback => gauntletFireCallback;
    [SerializeField]
    private UnityEvent fallingCallback;
    private UnityEvent FallingCallback => fallingCallback;
    [SerializeField]
    private UnityEvent landingCompletedCallback;
    private UnityEvent LandingCompletedCallback => landingCompletedCallback;
    [SerializeField]
    private UnityEvent pickupCallback;
    private UnityEvent PickupCallback => pickupCallback;
    [SerializeField]
    private UnityEvent pickupCompletedCallback;
    private UnityEvent PickupCompletedCallback => pickupCompletedCallback;

    public void OnGauntletFire()
    {
        GauntletFireCallback?.Invoke();
    }

    public void OnFalling()
    {
        FallingCallback?.Invoke();
    }

    public void OnLandingCompleted()
    {
        LandingCompletedCallback?.Invoke();
    }

    public void OnPickup()
    {
        PickupCallback?.Invoke();
    }

    public void OnPickupCompleted()
    {
        PickupCompletedCallback?.Invoke();
    }
}
