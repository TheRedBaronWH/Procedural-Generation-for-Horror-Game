using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
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
        CheckSprint();
        CheckCrouch();

        MoveCamera();
    }

    void FixedUpdate()
    {
        MoveBody();
    }

    private void CheckSprint()
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

    private void CheckCrouch()
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

    private void MoveBody()
    {
        Vector2 movement = Input.ReadMove();

        if (movement.sqrMagnitude > 0f)
        {
            movement = (_isCrouching ? _crouchModifier : 1f) * (_isSprinting ? _sprintModifier : 1f) * movement;
            Vector3 moveDirection = _transform.right * movement.x + _transform.forward * movement.y;
            _rb.MovePosition(_transform.position + _speed * Time.deltaTime * moveDirection);
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void MoveCamera()
    {
        Vector2 mouseDelta = Input.ReadMouseDelta();

        if (mouseDelta.sqrMagnitude > 0f)
        {
            _cameraAngle -= mouseDelta.y * _gameSettings.Sensitivity * _cameraUpSpeed * Time.deltaTime;
            _cameraAngle = Mathf.Clamp(_cameraAngle, _minCameraAngle, _maxCameraAngle);
            _cameraTransform.localRotation = Quaternion.Euler(_cameraAngle, 0f, 0f);
            _rb.MoveRotation(Quaternion.Euler(_transform.rotation.eulerAngles + Vector3.up * mouseDelta.x * _gameSettings.Sensitivity * _cameraLeftSpeed * Time.deltaTime));
        }
    }
}
