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
    [SerializeField]
    private float rotateSpeed;
    private float RotateSpeed => rotateSpeed;
    [SerializeField]
    private float cameraSwitchControlsDelay;
    private float CameraSwitchControlsDelay => cameraSwitchControlsDelay;


    private Transform self;
    private Transform Self => self ??= transform;
    private float DelayRemaining { get; set; }

    public void DelayControls()
    {
        DelayRemaining = CameraSwitchControlsDelay;
    }


    void Update()
    {
        if (DelayRemaining > 0)
        {
            DelayRemaining -= Time.deltaTime;
            CharacterController.SimpleMove(Self.forward * Speed);
            return;
        }

        Camera cam = CameraManagerSingleton.Instance.ActiveCamera;

        Vector2 inputVector = MoveAction.action.ReadValue<Vector2>();

        Transform camTransform = cam.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        float step = RotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(Self.forward, moveDirection, step, 0.0f);
        newDirection.y = 0;

        transform.rotation = Quaternion.LookRotation(newDirection);

        CharacterController.SimpleMove(moveDirection * Speed);
    }
}
