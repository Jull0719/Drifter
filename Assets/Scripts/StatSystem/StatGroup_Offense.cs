using System;
using UnityEngine;

[Serializable]
public class StatGroup_Offense
{
    public Stat damage;
    [Tooltip("暴击倍率 >= 1")] public Stat critPower;
    [Tooltip("暴击概率（0-60）")] public Stat critChance;
    [Tooltip("护甲穿透（0-1）")] public Stat armorReduction;
}
