using System;
using UnityEngine;

[Serializable]
public class StatGroup_Offense
{
    public Stat damage;
    [Tooltip("攻击速度（0-2）")] public Stat attackSpeed;
    [Tooltip("暴击倍率 >= 1")] public Stat critPower;
    [Tooltip("暴击概率（0-0.6）")] public Stat critChance;
    [Tooltip("护甲穿透（0-1）")] public Stat armorReduction;
}
