using System;
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
    private float animChangeSpeed;
    private float AnimChangeSpeed => animChangeSpeed;
    [SerializeField]
    private float cameraSwitchControlsDelay;
    private float CameraSwitchControlsDelay => cameraSwitchControlsDelay;
    [SerializeField]
    private Animator playerAnimator;
    private Animator PlayerAnimator => playerAnimator;


    private Transform self;
    private Transform Self => self ??= transform;
    private float DelayRemaining { get; set; }
    private bool IsControlsLocked { get; set; }

    private int VelocityZHash { get; set; }
    private int IsGroundedHash { get; set; }

    void Awake()
    {
        VelocityZHash = Animator.StringToHash("VelocityZ");
        IsGroundedHash = Animator.StringToHash("IsGrounded");
    }

    public void DelayControls()
    {
        DelayRemaining = CameraSwitchControlsDelay;
    }


    void Update()
    {


        if (IsControlsLocked)
        {
            // NOTE: Still need to call for gravity
            CharacterController.SimpleMove(Vector3.zero);
            //UpdateGrounded();
            return;
        }

        if (DelayRemaining > 0)
        {
            DelayRemaining -= Time.deltaTime;
            CharacterController.SimpleMove(Self.forward * Speed);
            return;
        }

        Camera cam = CameraManagerSingleton.Instance.ActiveCamera;

        Vector2 inputVector = MoveAction.action.ReadValue<Vector2>();

        float newAnimVelocityZ = Mathf.Lerp(PlayerAnimator.GetFloat(VelocityZHash), inputVector.magnitude, AnimChangeSpeed * Time.deltaTime);
        PlayerAnimator.SetFloat(VelocityZHash, newAnimVelocityZ);

        Transform camTransform = cam.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        Vector3 moveDirection = forward * inputVector.y + right * inputVector.x;
        moveDirection.y = 0;
        moveDirection.Normalize();

        // Vector3 forwardNormalized = Self.forward.normalized;
        // float rotationAmount = 1 - Vector3.Dot(forwardNormalized, moveDirection);
        // Debug.Log($"RM: {rotationAmount}");
        // float rotationAmountSigned = rotationAmount * Mathf.Sign(Vector3.SignedAngle(forwardNormalized, moveDirection, Vector3.up));
        // Debug.Log($"RMS: {rotationAmountSigned}");

        // if (inputVector.magnitude < 0.1f)
        // {
        //     rotationAmountSigned = 0;
        // }

        // PlayerAnimator.SetFloat("VelocityX", rotationAmountSigned);

        float step = RotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(Self.forward, moveDirection, step, 0.0f);
        newDirection.y = 0;

        transform.rotation = Quaternion.LookRotation(newDirection);

        CharacterController.SimpleMove(moveDirection * Speed);
        //UpdateGrounded();
    }

    void UpdateGrounded()
    {
        // TODO: This detection is super buggy, disabling for now
        bool isFalling = CharacterController.velocity.y < -0.05f;
        PlayerAnimator.SetBool(IsGroundedHash, !isFalling);
    }

    public void LockControls()
    {
        IsControlsLocked = true;
    }

    public void UnlockControls()
    {
        IsControlsLocked = false;
    }
}
