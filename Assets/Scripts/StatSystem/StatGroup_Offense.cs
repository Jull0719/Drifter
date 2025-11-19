using System;
using UnityEngine;

[Serializable]
public class StatGroup_Offense
{
    public Stat damage;
    public Stat critPower;
    [Tooltip("暴击概率（0-60）")] public Stat critChance;
}
