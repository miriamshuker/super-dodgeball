using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    [SerializeField] public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool ThrowWasPressed;
    public static bool ThrowIsHeld;
    public static bool ThrowWasReleased;


    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _throwAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _throwAction = PlayerInput.actions["Throw"];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        ThrowWasPressed = _throwAction.WasPressedThisFrame();
        ThrowIsHeld = _throwAction.IsPressed();
        ThrowWasReleased = _throwAction.WasReleasedThisFrame();
    }
}