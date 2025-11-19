using System;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;

    public float GetBaseValue() { return baseValue; }

    public void SetBaseValue(float value) { baseValue = value; }
}
