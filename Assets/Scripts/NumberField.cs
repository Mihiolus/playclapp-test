using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class NumberField : MonoBehaviour
{
    [SerializeField]
    private float _min = 0f, _max = 1f, _default = 0.5f;
    [SerializeField]
    private int _decimalPlaces = 1;
    private TMP_InputField field;
    public float CurrentValue { get; private set; }

    private void Awake()
    {
        Debug.Assert(_min < _max && _default > _min && _default < _max);

        field = GetComponent<TMP_InputField>();

        field.text = _default.ToString(GetFormatString());
        CurrentValue = _default;
    }

    private string GetFormatString()
    {
        return $"N{_decimalPlaces}";
    }

    public void OnEndEdit(string value)
    {
        float parsedValue;
        bool isFloat = float.TryParse(value, out parsedValue);
        if (!isFloat || parsedValue < _min || parsedValue > _max)
        {
            field.text = CurrentValue.ToString(GetFormatString());
        }
        else
        {
            field.text = parsedValue.ToString(GetFormatString());
            CurrentValue = parsedValue;
        }
    }
}
