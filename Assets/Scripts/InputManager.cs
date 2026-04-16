using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    
    [SerializeField] 
    public PlayerInput PlayerInput;

    public Vector2 Movement;
    public bool JumpWasPressed;
    public bool JumpIsHeld;
    public bool JumpWasReleased;
    public bool ThrowWasPressed;
    public bool ThrowIsHeld;
    public bool ThrowWasReleased;


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

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        ThrowWasPressed = _throwAction.WasPressedThisFrame();
        ThrowIsHeld = _throwAction.IsPressed();
        ThrowWasReleased = _throwAction.WasReleasedThisFrame();
    }

    public void UpdateMoveValues(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
        //Debug.Log("I am " + this.name + " and my movement vector is " + Movement);
    }
}