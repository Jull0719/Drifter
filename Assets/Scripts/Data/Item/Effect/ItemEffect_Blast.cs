using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Inventory/Item Effect/Blast Effect", fileName = "Item Effect - Blast")]
public class ItemEffect_Blast : ItemEffectDataSO
{
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float blastDamage;

    [Space]
    [SerializeField] private float healthPercentageTrigger;
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    [Header("VFX")]
    [SerializeField] private GameObject blastPrefab;
    [SerializeField] private GameObject onHitVfxPrefab;

    private void OnEnable()
    {
        lastTimeUsed = -999;
    }

    public override void Execute()
    {
        bool noCooldown = Time.time > lastTimeUsed + cooldown;
        bool reachedThreshold = player.health.GetHealthPercent() < healthPercentageTrigger;
        if (noCooldown && reachedThreshold)
        {
            player.vfx.CreateEffectVFX(blastPrefab, player.transform);
            DamageEnemiesWithBlast();
            lastTimeUsed = Time.time;
        }
    }

    // 冲击波对敌人造成伤害
    private void DamageEnemiesWithBlast()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(player.transform.position, 2, targetLayer);

        foreach (var target in enemies)
        {
            // 对敌人造成伤害
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;
            bool targetGotHit = damagable.TakeDamage(blastDamage, player.transform);

            if (targetGotHit)
            {
                // 击中效果
                player.vfx.CreateEffectVFX(onHitVfxPrefab, target.transform);

                // 使敌人晕眩
                ICounterable counterable = target.GetComponent<ICounterable>();
                if (counterable == null) continue;
                counterable.HandleCountered();
            }
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.health.OnTakingDamage += Execute;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.health.OnTakingDamage -= Execute;
        player = null;
    }
}
