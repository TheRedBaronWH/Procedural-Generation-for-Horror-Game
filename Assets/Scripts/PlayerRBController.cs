using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerRBController : MonoBehaviour
{
    private readonly InputReader Input = InputReader.Instance;

    [Header("Player Settings")]
    [SerializeField] private GameSettings _gameSettings = null;
    [SerializeField] private Rigidbody _rb = null;
    [SerializeField] private Transform _transform = null;
    [SerializeField] private Transform _cameraTransform = null;

    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _sprintModifier = 2f;
    [SerializeField] private float _crouchModifier = 0.5f;
    [SerializeField] private float _crouchHeight = 1f;
    [SerializeField] private float _jumpForce = 5f;

    private bool _isSprinting = false;
    private bool _isCrouching = false;

    private Vector3 _baseScale;

    [Header("Camera Settings")]
    [SerializeField] private float _cameraUpSpeed = 50f;
    [SerializeField] private float _cameraLeftSpeed = 80f;
    [SerializeField] private float _minCameraAngle = 90f;
    [SerializeField] private float _maxCameraAngle = -90f;
    [ShowInInspector][ReadOnly] private float _cameraAngle = 0f;

    private Vector2 moveInput = new();
    private Vector2 mouseDelta = new();

    void Start()
    {
        if(_gameSettings == null)
        {
            Debug.LogError($"{gameObject.name} : GameSettings reference is missing");
        }

        if(_rb == null)
        {
            Debug.LogError($"{gameObject.name} : Rigidbody reference is missing");
        }
        
        if(_transform == null)
        {
            Debug.LogError($"{gameObject.name} : Transform reference is missing");
        }

        if(_cameraTransform == null)
        {
            Debug.LogError($"{gameObject.name} : Camera Transform reference is missing");
        }

        _baseScale = _transform.localScale;
    }

    void Update()
    {
        ReadInputs();
        Sprint();
        Crouch();

        RotateCamera();
    }

    void FixedUpdate()
    {
        MoveBody();
        Jump();
    }

    private void ReadInputs()
    {
        moveInput = Input.ReadMove();
        mouseDelta = Input.ReadLook();
    }

    private void Sprint()
    {
        if (_gameSettings.HoldSprint)
        {
            _isSprinting = Input.ReadSprint();
        }
        else
        {
            if (Input.ReadSprint())
            {
                _isSprinting = !_isSprinting;
            }
        }
    }

    private void Crouch()
    {
        if (_gameSettings.HoldCrouch)
        {
            _isCrouching = Input.ReadCrouch();
        }
        else
        {
            if (Input.ReadCrouch())
            {
                _isCrouching = !_isCrouching;
            }
        }

        transform.localScale = new Vector3(_baseScale.x, _isCrouching ? _crouchHeight : _baseScale.y, _baseScale.z);
    }

    private void Jump()
    {
        if(Input.ReadJump() && _rb.linearVelocity.y < 0.1f)
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    private void MoveBody()
    {
        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();   
        }

        moveInput = (_isCrouching ? _crouchModifier : 1f) * (_isSprinting ? _sprintModifier : 1f) * moveInput;
        Vector3 moveDirection = _transform.right * moveInput.x + _transform.forward * moveInput.y;
        _rb.linearVelocity = _speed * moveDirection;
    }

    private void RotateCamera()
    {
        if (mouseDelta.sqrMagnitude > 1f)
        {
            mouseDelta.Normalize();
        }

        _cameraAngle -= mouseDelta.y * _gameSettings.Sensitivity * _cameraUpSpeed * Time.deltaTime;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _minCameraAngle, _maxCameraAngle);
        _cameraTransform.localRotation = Quaternion.Euler(_cameraAngle, 0f, 0f);

        float rotation = mouseDelta.x * _gameSettings.Sensitivity * _cameraLeftSpeed * Time.deltaTime;
        _rb.MoveRotation(_rb.rotation * Quaternion.Euler(0f, rotation, 0f));
    }
}
