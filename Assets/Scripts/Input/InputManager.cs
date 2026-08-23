using UnityEngine;

public class InputManager : MonoBehaviour
{
    private NoStepsOnlyLeapsInputActions inputActions;

    public float LookInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool JumpHeld { get; private set; }
    public bool ReturnToCheckpointPressed { get; private set; }
    public bool PausePressed { get; private set; }

    private void Awake()
    {
        inputActions = new NoStepsOnlyLeapsInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Jump.started += OnJumpStarted;
        inputActions.Player.Jump.canceled += OnJumpCanceled;
        inputActions.Player.ReturnToCheckpoint.started += OnReturnToCheckpointStarted;
        inputActions.Player.Pause.started += OnPauseStarted;
    }

    private void OnDisable()
    {
        inputActions.Player.Jump.started -= OnJumpStarted;
        inputActions.Player.Jump.canceled -= OnJumpCanceled;
        inputActions.Player.ReturnToCheckpoint.started -= OnReturnToCheckpointStarted;
        inputActions.Player.Pause.started -= OnPauseStarted;

        inputActions.Disable();
    }

    private void Update()
    {
        LookInput = inputActions.Player.Look.ReadValue<float>();
    }

    private void LateUpdate()
    {
        JumpPressed = false;
        JumpReleased = false;
        ReturnToCheckpointPressed = false;
        PausePressed = false;
    }

    private void OnJumpStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        JumpPressed = true;
        JumpHeld = true;
    }

    private void OnJumpCanceled(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        JumpReleased = true;
        JumpHeld = false;
    }

    private void OnReturnToCheckpointStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        ReturnToCheckpointPressed = true;
    }

    private void OnPauseStarted(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        PausePressed = true;
    }
}