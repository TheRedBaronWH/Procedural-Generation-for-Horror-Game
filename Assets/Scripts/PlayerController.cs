using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputReader InputReader => InputReader.Instance;

    [Header("Player Settings")]
    [SerializeField] private GameSettings _gameSettings = null;
    [SerializeField] private CharacterController _characterController = null;
    [SerializeField] private Transform _transform = null;
    [SerializeField] private Transform _cameraTransform = null;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintModifier = 2f;
    [SerializeField] private float _crouchModifier = 0.5f;
    [SerializeField] private float _crouchHeight = 1f;
    private Vector3 _baseScale;

    [Header("Jump Settings")]
    [SerializeField] private bool _jumpingEnabled = false;
    [SerializeField] private float _jumpHeight = 1.5f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _fallbackVerticalVelocity = -2f;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraUpSpeed = 50f;
    [SerializeField] private float _cameraLeftSpeed = 80f;
    [SerializeField] private float _minCameraAngle = -90f;
    [SerializeField] private float _maxCameraAngle = 90f;

    [Header("Read Inputs")]
    [ShowInInspector][ReadOnly] private float _cameraAngle = 0f;
    [ShowInInspector][ReadOnly] private bool _isSprinting = false;
    [ShowInInspector][ReadOnly] private bool _isCrouching = false;
    [ShowInInspector][ReadOnly] private float _verticalVelocity = 0f;


    void Start()
    {
        if (_gameSettings == null)
        {
            Debug.LogError($"{gameObject.name} : GameSettings reference is missing");
        }

        if (_characterController == null)
        {
            Debug.LogError($"{gameObject.name} : CharacterController reference is missing");
        }

        if (_transform == null)
        {
            Debug.LogError($"{gameObject.name} : Transform reference is missing");
        }

        if (_cameraTransform == null)
        {
            Debug.LogError($"{gameObject.name} : Camera Transform reference is missing");
        }

        _baseScale = _transform.localScale;
    }

    void Update()
    {
        UpdateSprint();
        UpdateCrouch();

        RotateCamera();
        MoveBody();
    }

    private void UpdateSprint()
    {
        if (_gameSettings.HoldSprint)
        {
            _isSprinting = InputReader.ReadSprint();
        }
        else
        {
            if (InputReader.ReadSprint())
            {
                _isSprinting = !_isSprinting;
            }
        }
    }

    private void UpdateCrouch()
    {
        if (_gameSettings.HoldCrouch)
        {
            _isCrouching = InputReader.ReadCrouch();
        }
        else
        {
            if (InputReader.ReadCrouch())
            {
                _isCrouching = !_isCrouching;
            }
        }

        transform.localScale = new Vector3(_baseScale.x, _isCrouching ? _crouchHeight : _baseScale.y, _baseScale.z);
    }

    private void MoveBody()
    {
        Vector2 moveInput = InputReader.ReadMove();

        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }
        moveInput = (_isCrouching ? _crouchModifier : 1f) * (_isSprinting ? _sprintModifier : 1f) * moveInput;
        Vector3 moveDirection = (_transform.right * moveInput.x + _transform.forward * moveInput.y) * _speed;

        if(_jumpingEnabled)
        {
            if (_characterController.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = _fallbackVerticalVelocity;
            }
            if (InputReader.ReadJump() && _characterController.isGrounded)
            {
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * 2f * Mathf.Abs(_gravity));
            }
            _verticalVelocity += _gravity * Time.deltaTime;

            moveDirection += Vector3.up * _verticalVelocity;
        }

        _characterController.Move(moveDirection * Time.deltaTime);
    }

    private void RotateCamera()
    {
        Vector2 mouseDelta = InputReader.ReadLook();

        if (mouseDelta.sqrMagnitude > 1f)
        {
            mouseDelta.Normalize();
        }

        _cameraAngle -= mouseDelta.y * _gameSettings.Sensitivity * _cameraUpSpeed * Time.deltaTime;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _minCameraAngle, _maxCameraAngle);
        _cameraTransform.localRotation = Quaternion.Euler(_cameraAngle, 0f, 0f);

        float rotation = mouseDelta.x * _gameSettings.Sensitivity * _cameraLeftSpeed * Time.deltaTime;
        _transform.Rotate(Vector3.up * rotation);
    }
}
