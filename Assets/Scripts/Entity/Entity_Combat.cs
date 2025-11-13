using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [Header("攻击数值")]
    [SerializeField] protected float damage = 10;

    [Header("攻击检测")]
    [SerializeField] protected float attackRadius = 1;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;

    protected Entity entity;

    private void Awake()
    {
        entity = GetComponent<Entity>();
    }

    // 实施攻击
    public virtual void PerformAttack()
    {
        foreach (var target in TargetDetected())
        {
            IDamagable damagable = target.GetComponent<IDamagable>();
            damagable?.TakeDamage(damage, entity);
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
