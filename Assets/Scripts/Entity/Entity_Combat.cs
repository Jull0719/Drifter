using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("攻击检测")]
    [SerializeField] protected float attackRadius = 1;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;

    protected Entity entity;
    protected Entity_VFX vfx;
    protected Entity_Stats stats;

    private void Awake()
    {
        entity = GetComponent<Entity>();
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

            float physcialDamage = stats.GetPhysicalDamage();
            targetGotHit = damagable.TakeDamage(physcialDamage, entity.transform);

            if (targetGotHit)
                vfx?.CreateOnHitVfx(target.transform, false, physcialDamage);

            //Entity_Health health = target.GetComponent<Entity_Health>();
            //if (health == null) continue;
            //bool isHeavyHit = health.IsHeavyHit(physcialDamage);
        }
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
