using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonController : MonoBehaviour
{  
    // Serialized variables
    [Header("Movement speed")]
    [SerializeField] private float _walkSpeed = 3.0f;

    [Header("Look paramaters")]
    [SerializeField] private float _mouseSensitivity = 0.1f;
    [SerializeField] private float _upDownLookRange = 80.0f;

    [Header("Jump paramaters")]
    [SerializeField] private float _jumpForce = 5.0f;
    [SerializeField] private float _gravityMultiplier = 1.0f;

    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerInputManager _playerInputManager;

    // Private variables
    private Vector3 _currentMovement;
    private float _verticalRotation;
    private float _currentSpeed;




    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _currentSpeed = _walkSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleJumping();
    }


    // movement functions
    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(_playerInputManager.MovementInput.x, 0.0f, _playerInputManager.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        worldDirection.Normalize();

        return worldDirection;
    }

    private void HandleJumping()
    {
        if (_characterController.isGrounded)
        {
            _currentMovement.y = -1.5f;

            if(_playerInputManager.IsJumpTriggered)
            {
                _currentMovement.y = _jumpForce;
            }
        }
        else
        {
            _currentMovement.y += Physics.gravity.y * _gravityMultiplier * Time.deltaTime;
        }
    }

    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();
        _currentMovement.x = worldDirection.x * _currentSpeed;
        _currentMovement.z = worldDirection.z * _currentSpeed;

        HandleJumping();
        _characterController.Move(_currentMovement * Time.deltaTime);
    }


    // Rotation Functions
    private void ApplyHorizontalRotation(float rotationAmount)
    {
        transform.Rotate(0.0f, rotationAmount, 0.0f);
    }

    private void ApplyVerticalRotation(float rotationAmount)
    {
        _verticalRotation = Mathf.Clamp(_verticalRotation - rotationAmount, -_upDownLookRange, _upDownLookRange);
        _mainCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0.0f, 0.0f);
    }

    private void HandleRotation()
    {
        float mouseXRotation = _playerInputManager.RotationInput.x * _mouseSensitivity;
        float mouseYRotation = _playerInputManager.RotationInput.y * _mouseSensitivity;

        ApplyHorizontalRotation(mouseXRotation);
        ApplyVerticalRotation(mouseYRotation);
    }
}
