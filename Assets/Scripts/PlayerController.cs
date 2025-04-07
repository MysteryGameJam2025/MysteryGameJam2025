using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : AbstractMonoBehaviourSingleton<PlayerController>
{
    [SerializeField]
    private InputActionReference moveAction;
    private InputActionReference MoveAction => moveAction;
    [SerializeField]
    private CharacterController characterController;
    private CharacterController CharacterController => characterController;
    [SerializeField]
    private float speed;
    private float Speed => speed;


    void Update()
    {
        Camera cam = CameraManagerSingleton.Instance.ActiveCamera;

        Vector2 inputVector = MoveAction.action.ReadValue<Vector2>();

        Transform camTransform = cam.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        CharacterController.SimpleMove(moveDirection * Speed);
    }
}
