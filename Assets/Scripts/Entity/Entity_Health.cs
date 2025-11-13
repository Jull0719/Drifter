using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamagable
{
    [Header("击退")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(3, 2);
    [SerializeField] protected float knockbackDuration = 0.15f;
    [Header("重击")]
    [SerializeField] protected float heavyThresholder = 0.3f; // 伤害占最大生命值的30%
    [SerializeField] protected Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] protected float heavyKnockbackDuration = 0.2f;

    [Header("血量设置")]
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;
    protected bool isDead;
    [Space]

    protected Entity entity;
    protected Entity_VFX vfx;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        vfx = GetComponent<Entity_VFX>();

        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    // 受击
    public virtual void TakeDamage(float damage, Entity damageDealer)
    {
        //Debug.Log("Entity - " + gameObject.name + "受到攻击");
        if (isDead) return;

        ReduceHealth(damage);

        // 击退
        Vector2 knockback = CalculateKnockbackPower(damage, damageDealer);
        float duration = CalculateKnockbackDuration(damage);
        entity.ReceiveKnockback(knockback, duration);
        vfx.OnDamageVfx();
    }

    private void ReduceHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " is Died.");
        OnDie();
    }

    public virtual void OnDie()
    {

    }

    // 计算受击占总生命值比例
    private bool IsHeavyHit(float damage) => damage / maxHealth > heavyThresholder;

    // 计算击退时间
    private float CalculateKnockbackDuration(float damage) => IsHeavyHit(damage) ? heavyKnockbackDuration : knockbackDuration;

    // 计算击退力
    private Vector2 CalculateKnockbackPower(float damage, Entity damageDealer)
    {
        int direction = transform.position.x > damageDealer.transform.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyHit(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;
        return knockback;
    }
}
