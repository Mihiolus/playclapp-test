using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class NumberField : MonoBehaviour
{
    [SerializeField]
    private float _min = 0f, _max = 1f, _default = 0.5f;
    [SerializeField]
    private int _decimalPlaces = 1;
    private TMP_InputField field;

    private float _currentValue;

    public float CurrentValue
    {
        get => _currentValue;
        private set
        {
            _currentValue = (float)Math.Round(value, _decimalPlaces);
        }
    }

    private void Awake()
    {
        Debug.Assert(_min < _max && _default > _min && _default < _max);

        field = GetComponent<TMP_InputField>();

        CurrentValue = _default;
        UpdateDisplay();
    }

    public void OnEndEdit(string value)
    {
        float parsedValue;
        bool isFloat = float.TryParse(value, out parsedValue);
        if (!isFloat || parsedValue < _min || parsedValue > _max)
        {
            UpdateDisplay();
        }
        else
        {
            CurrentValue = parsedValue;
            UpdateDisplay();
            SFXPlayer.Play(SFXPlayer.SoundType.EndEdit);
        }
    }

    private void UpdateDisplay()
    {
        field.text = CurrentValue.ToString();
    }
}
