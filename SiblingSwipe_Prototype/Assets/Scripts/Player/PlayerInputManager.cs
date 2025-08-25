using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    // Serialized variables
    [Header("Input action asset")]
    [SerializeField] private InputActionAsset _playerControls;

    [Header("Action map name reference")]
    [SerializeField] private string _actionmapName = "Player";

    [Header("Action name reference")]
    [SerializeField] private string _movement = "movement";
    [SerializeField] private string _rotation = "Rotation";
    [SerializeField] private string _jump = "Jump";

    [Header("Deadzone value")]
    [SerializeField] private float _leftStickDeadzonevalue;

    // Private variables
    private InputAction _movementAction;
    private InputAction _rotationAction;
    private InputAction _jumpAction;


    // Getters and setters
    public Vector2 MovementInput { get; private set; }
    public Vector2 RotationInput { get; private set; }
    public bool IsJumpTriggered { get; private set ; }




    private void Awake()
    {
        InputActionMap mapReference = _playerControls.FindActionMap(_actionmapName);

        // Get the actions
        _movementAction = mapReference.FindAction(_movement);
        _rotationAction = mapReference.FindAction(_rotation);
        _jumpAction = mapReference.FindAction(_jump);

        RegisterInputActions();

        // Handling deadzones
        InputSystem.settings.defaultDeadzoneMin = _leftStickDeadzonevalue;
    }

    private void RegisterInputActions()
    {
        _movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        _movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        _rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        _rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        _jumpAction.performed += inputInfo => IsJumpTriggered = true;
        _jumpAction.canceled += inputInfo => IsJumpTriggered = false;

    }

    private void OnEnable()
    {
        _playerControls.FindActionMap(_actionmapName).Enable();
    }

    private void OnDisable()
    {
        _playerControls.FindActionMap(_actionmapName).Disable();
    }

}
