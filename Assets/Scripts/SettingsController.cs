using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private GameSettings _gameSettings = null;
    [SerializeField] private FloatSlider _sensitivitySlider = null;
    [SerializeField] private IntegerSlider _volumeSlider = null;
    [SerializeField] private ButtonToggle _sprintToggle = null;
    [SerializeField] private ButtonToggle _crouchToggle = null;

    [ShowInInspector]
    [ReadOnly]
    public static event Action OnBackClicked;

    void Start()
    {
        if(_gameSettings == null)
        {
            Debug.LogError($"{gameObject.name} :  GameSettings reference is missing");
        }

        if (_volumeSlider == null)
        {
            Debug.LogError($"{gameObject.name} : VolumeSlider reference is missing");
        }
        else
        {
            _volumeSlider.Value = _gameSettings.Volume;
        }

        if (_sensitivitySlider == null)
        {
            Debug.LogError($"{gameObject.name} : SensitivitySlider reference is missing");
        }
        else
        {
            _sensitivitySlider.Value = _gameSettings.Sensitivity;
        }

        if (_sprintToggle == null)
        {
            Debug.LogError($"{gameObject.name} : SprintToggle reference is missing");
        }
        else
        {
            _sprintToggle.Value = _gameSettings.HoldSprint;
        }

        if (_crouchToggle == null)
        {
            Debug.LogError($"{gameObject.name} : CrouchToggle reference is missing");
        }
        else
        {
            _crouchToggle.Value = _gameSettings.HoldCrouch;
        }
    }

    void Update() 
    { 
        if(InputReader.Instance.ReadEscape())
        {
            Back();
        }
    }

    public void OnVolumeChange()
    {
        _gameSettings.Volume = _volumeSlider.Value;
    }

    public void OnSensitivityChange()
    {
        _gameSettings.Sensitivity = _sensitivitySlider.Value;
    }

    public void OnSprintToggle()
    {
        _gameSettings.HoldSprint = _sprintToggle.Value;
    }

    public void OnCrouchToggle()
    {
        _gameSettings.HoldCrouch = _crouchToggle.Value;
    }

    public void Back()
    {
        SceneLoader.Instance.UnloadSettings();
        OnBackClicked?.Invoke();
    }
}
