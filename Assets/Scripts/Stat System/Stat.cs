using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    [SerializeField] private float baseValue;
    [SerializeField] private List<StatModifier> modifiers = new List<StatModifier>();
    private float finalValue;
    private bool needToCalculate = true;
    public void SetBaseValue(float value) { baseValue = value; }

    public float GetValue()
    {
        if (needToCalculate)
        {
            finalValue = GetFinalValue();
            needToCalculate = false;
        }
        return finalValue;
    }

    public float GetFinalValue()
    {
        float finalValue = baseValue;

        foreach (var modifier in modifiers)
        {
            finalValue += modifier.value;
        }

        return finalValue;
    }

    // 添加属性修改器
    public void AddModifiers(float value, string source)
    {
        StatModifier modifierToAdd = new StatModifier(value, source);
        modifiers.Add(modifierToAdd);
        needToCalculate = true;
    }

    // 移除属性修改器
    public void RemoveModifiers(string source)
    {
        modifiers.RemoveAll(modifier => modifier.source == source);
        needToCalculate = true;
    }
}

[Serializable]
public class StatModifier
{
    public float value;
    public string source;
    public StatModifier(float value, string source)
    {
        this.value = value;
        this.source = source;
    }
}
