using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect/Heal Effect", fileName = "Item Effect - Heal")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healthPercent = 0.1f;

    private Player player;

    public override bool CanBeUsed()
    {
        player = FindAnyObjectByType<Player>();

        if (player.health.GetCurrentHealth() < player.stats.GetMaxHealth())
            return true;
        else
        {
            Debug.Log("生命值已满");
            return false;
        }
    }

    public override void Execute()
    {
        float healthAmount = player.stats.GetMaxHealth() * healthPercent;
        player.health.IncreaseHealth(healthAmount);
    }
}
