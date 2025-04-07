using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Transform PlayerTransform => playerTransform ??= PlayerController.Instance.transform;
    private Transform self;
    private Transform Self => self ??= transform;
    void Update()
    {
        Self.LookAt(PlayerTransform.position);
    }
}
