using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
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

    [SerializeField] float maxValue = 100f;
    [SerializeField] float minValue = 0f;

    public event Action<float> OnValueChanged;

    void Start()
    {
        Value = maxValue;
    }

    public void Add(float amount)
    {
        Value = Mathf.Min(Value + amount, maxValue);
    }
    
    public void Subtract(float amount)
    {
        Value = Mathf.Max(Value - amount, minValue);
    }
}
