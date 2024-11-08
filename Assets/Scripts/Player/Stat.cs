using System;
using UnityEngine;

[Serializable]
public class Stat
{
    public StatType type;
    public float maxValue = 100f;
    float minValue = 0f;
    
    float _value;
    public float Value 
    {
        get {return _value;}
        set
        {
            _value = value;
            OnValueChanged?.Invoke(_value / maxValue);
        }
    }

    public event Action<float> OnValueChanged;

    public void Add(float amount)
    {
        Value = Mathf.Min(Value + amount, maxValue);
    }
    
    public void Subtract(float amount)
    {
        Value = Mathf.Max(Value - amount, minValue);
    }
}
