using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMPro.TMP_InputField))]
public class NumberField : MonoBehaviour
{
    [SerializeField]
    private float _min = 0f, _max = 1f;
    [SerializeField]
    private int _decimalPlaces = 1;
    private TMP_InputField field;
    private float _oldValue;

    private void Awake()
    {
        field = GetComponent<TMP_InputField>();

        bool isFloat = float.TryParse(field.text, out _oldValue);
        if (!isFloat)
        {
            _oldValue = _min;
        }
    }

    private string GetFormatString()
    {
        return $"N{_decimalPlaces}";
    }

    public void OnEndEdit(string value)
    {
        float parsedValue;
        bool isFloat = float.TryParse(value, out parsedValue);
        if (!isFloat)
        {
            field.text = _oldValue.ToString(GetFormatString());
        }
        if (parsedValue < _min)
        {
            field.text = _min.ToString(GetFormatString());
        }
        else if (parsedValue > _max)
        {
            field.text = _max.ToString(GetFormatString());
        }
        else
        {
            field.text = parsedValue.ToString(GetFormatString());
        }
    }
}
