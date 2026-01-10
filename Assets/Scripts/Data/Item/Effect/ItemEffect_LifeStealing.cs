using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect/Life Stealing Effect", fileName = "Item Effect - Life Stealing")]
public class ItemEffect_LifeStealing : ItemEffectDataSO
{
    [SerializeField] private float lifeStealingPercent = 0.2f;

    private void LifeStealing(float damage)
    {
        player.health.IncreaseHealth(lifeStealingPercent * damage);
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnPerformingAttack += LifeStealing;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnPerformingAttack -= LifeStealing;
        player = null;
    }
}
