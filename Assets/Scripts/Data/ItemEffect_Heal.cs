using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect/Heal Effect", fileName = "Item Effect - Heal")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healthPercent = 0.1f;

    public override void Execute()
    {
        Player player = FindAnyObjectByType<Player>();

        float healthAmount = player.stats.GetMaxHealth() * healthPercent;
        player.health.IncreaseHealth(healthAmount);
    }
}
