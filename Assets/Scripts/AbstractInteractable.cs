using UnityEngine;

public abstract class AbstractInteractable : MonoBehaviour
{
    public abstract void OnInteractionHoverStart();
    public abstract void OnInteractionHoverEnd();
    public abstract void OnInteract();
}