using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    [SerializeField]
    private float delay;
    private float Delay => delay;

    void Awake()
    {
        Destroy(gameObject, Delay);
    }
}
