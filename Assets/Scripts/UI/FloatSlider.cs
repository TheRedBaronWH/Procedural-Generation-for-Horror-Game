using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FloatSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider = null;
    [SerializeField] private TMP_InputField _inputField = null;

    [SerializeField] private float _minValue = 0f;
    [SerializeField] private float _maxValue = 1f;
    [SerializeField] private float _value = 1f;
    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            _value = Mathf.Clamp(_value, _minValue, _maxValue);
            _value = MathF.Round(_value, 2);
            _slider.value = _value;
            _inputField.text = _value.ToString();
            OnValueChanged?.Invoke();
        }
    }

    [SerializeField] private UnityEvent OnValueChanged;

    void Awake()
    {
        if (_slider == null)
        {
            Debug.LogError($"{gameObject.name} : ButtonText reference is missing");
        }
        else
        {
            _slider.value = _value;
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _slider.wholeNumbers = false;
        }

        if (_inputField == null)
        {
            Debug.LogError($"{gameObject.name} : ButtonImage reference is missing");
        }
        else
        {
            _inputField.text = _value.ToString();
        }
    }

    public void OnSliderValueChanged()
    {
        Value = _slider.value;
    }

    public void OnInputfieldValueChanged()
    {
        Value = float.Parse(_inputField.text);
    }
}
