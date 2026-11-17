using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    [SerializeField] private Image ButtonImage = null;
    [SerializeField] private TextMeshProUGUI ButtonText = null;
    [SerializeField] private string _onText = "On";
    [SerializeField] private string _offText = "Off";
    [SerializeField] private Color _onColor = Color.white;
    [SerializeField] private Color _offColor = Color.gray;

    private bool _value = true;
    public bool Value { 
        get => _value;
        set
        {
            _value = value;
            ButtonText.text = _value ? _onText : _offText;
            ButtonImage.color = _value ? _onColor : _offColor;
            OnValueChanged?.Invoke();
        }
    }
    [SerializeField] private UnityEvent OnValueChanged;

    void Awake()
    {
        if (ButtonText == null)
        {
            Debug.LogError($"{gameObject.name} : ButtonText reference is missing");
        }
        else
        {
            ButtonText.text = _value ? _onText : _offText;
        }

        if (ButtonImage == null)
        {
            Debug.LogError($"{gameObject.name} : ButtonImage reference is missing");
        }   
        else
        {
            ButtonImage.color = _value ? _onColor : _offColor;
        }
    }

    public void ToggleValue()
    {
        Value = !Value;
    }
}
