using System;
using UnityEngine;

[Serializable]
public class StatGroup_Defense
{
    public Stat armor;
    [Tooltip("闪避概率（0-65）")] public Stat evasion;
}
