using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnPerformingAttack;

    [Header("攻击检测")]
    [SerializeField] protected float attackRadius = 1;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;

    protected Entity_SFX sfx;
    protected Entity_VFX vfx;
    protected Entity_Stats stats;

    private void Awake()
    {
        sfx = GetComponentInChildren<Entity_SFX>();
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    // 实施攻击
    public virtual void PerformAttack()
    {
        bool targetGotHit = false;
        foreach (var target in TargetDetected())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            if (damagable == null) continue;

            float physcialDamage = stats.GetPhysicalDamage(out bool isCrit);
            targetGotHit = damagable.TakeDamage(physcialDamage, transform);

            if (targetGotHit)
            {
                OnPerformingAttack?.Invoke(physcialDamage);
                vfx?.CreateOnHitVfx(target.transform, isCrit);
                sfx?.PlayAttackHit();
            }
        }

        if (!targetGotHit)
            sfx?.PlayAttackMiss();
    }

    // 攻击检测
    public Collider2D[] TargetDetected()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, targetLayer);
    }

    private void OnDrawGizmos()
    {
        // 攻击
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
