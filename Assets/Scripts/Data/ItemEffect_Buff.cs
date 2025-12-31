using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect/Buff Effect", fileName = "Item Effect - Buff")]
public class ItemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffData[] buffs;
    [SerializeField] private float buffDuration;
    [SerializeField] private string buffSource = Guid.NewGuid().ToString();

    private Player_Stats playerStats;

    public override bool CanBeUsed()
    {
        playerStats = FindAnyObjectByType<Player_Stats>();

        if (playerStats.CanApplyBuff(buffSource))
            return true;
        else
        {
            Debug.Log("不能附加同样的Buff");
            return false;
        }
    }

    public override void Execute()
    {
        playerStats.ApplyBuffs(buffs, buffDuration, buffSource);
    }
}
