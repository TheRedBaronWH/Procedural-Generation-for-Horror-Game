using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IntegerSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider = null;
    [SerializeField] private TMP_InputField _inputField = null;

    [SerializeField] private int _minValue = 0;
    [SerializeField] private int _maxValue = 100;
    [SerializeField] private int _value = 100;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            _value = Mathf.Clamp(_value, _minValue, _maxValue);
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
            _slider.wholeNumbers = true;
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
        Value = (int)_slider.value;
    }

    public void OnInputfieldValueChanged()
    {
        Value = int.Parse(_inputField.text);
    }
}

