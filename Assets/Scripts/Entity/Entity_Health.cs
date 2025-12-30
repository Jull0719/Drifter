using System;
using TMPro;
using UnityEngine;

public class Entity_Health : MonoBehaviour, IDamagable
{
    public event Action OnHealthChange;

    [Header("击退")]
    [SerializeField] protected Vector2 knockbackPower = new Vector2(3, 2);
    [SerializeField] protected float knockbackDuration = 0.15f;
    [Header("重击")]
    [SerializeField] protected float heavyThresholder = 0.3f; // 伤害占最大生命值的30%
    [SerializeField] protected Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] protected float heavyKnockbackDuration = 0.2f;

    [Header("血量设置")]
    [SerializeField] protected float currentHealth;
    protected bool isDead;
    [Space]
    [Header("回血")]
    [SerializeField] protected bool canRegenerateHealth = true;
    [SerializeField] protected float healthRegenInterval;

    protected Entity entity;
    protected Entity_VFX vfx;
    protected Entity_Stats stats;

    protected Rigidbody2D rb;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();

        rb = GetComponent<Rigidbody2D>();

        currentHealth = stats.GetMaxHealth();
    }

    protected virtual void Start()
    {
        InvokeRepeating(nameof(RegenerateHealth), 0, healthRegenInterval);
    }

    // 受击
    public virtual bool TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead)
        {
            Debug.Log(gameObject.name + " is Died.");
            return false;
        }

        if (AttackEvaded())
        {
            Debug.Log($"{gameObject.name} 闪避攻击");

            return false;
        }

        //Debug.Log("Entity - " + gameObject.name + "受到攻击");

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats ? attackerStats.GetArmorReduction() : 0;

        float armorMitigation = stats.GetArmorMitigation(armorReduction);
        float finalPhysicalDamage = damage * (1 - armorMitigation);

        ReduceHealth(finalPhysicalDamage);

        TakeKnockback(finalPhysicalDamage, damageDealer);
        vfx.OnDamageVfx();

        return true;
    }

    // 闪避
    protected bool AttackEvaded() => UnityEngine.Random.Range(0, 100) < stats.GetEvasion() * 100;

    // 生命值再生
    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
            return;

        IncreaseHealth(stats.GetRegenerateHealth());
    }

    // 生命值增加
    public void IncreaseHealth(float healthAmount)
    {
        if (isDead)
            return;

        float newHealth = currentHealth + healthAmount;
        float maxHealth = stats.GetMaxHealth();

        currentHealth = Mathf.Min(maxHealth, newHealth);
        OnHealthChange?.Invoke();
    }

    // 生命值减少
    private void ReduceHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
        OnHealthChange?.Invoke();
    }

    protected virtual void Die()
    {
        isDead = true;
        currentHealth = 0;

        OnDie();
    }

    public virtual void OnDie()
    {

    }

    // 获取生命值
    public float GetCurrentHealth() => currentHealth;

    // 计算现在的生命值占比
    public float GetHealthPercent()
    {
        return currentHealth / stats.GetMaxHealth();
    }

    // 计算受击占总生命值比例
    public bool IsHeavyHit(float damage) => damage / stats.GetMaxHealth() >= heavyThresholder;

    // 击退
    private void TakeKnockback(float damage, Transform damageDealer)
    {
        // 击退
        Vector2 knockback = CalculateKnockbackPower(damage, damageDealer);
        float duration = CalculateKnockbackDuration(damage);
        entity.ReceiveKnockback(knockback, duration);
    }
    // 计算击退时间
    private float CalculateKnockbackDuration(float damage) => IsHeavyHit(damage) ? heavyKnockbackDuration : knockbackDuration;
    // 计算击退力
    private Vector2 CalculateKnockbackPower(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;

        Vector2 knockback = IsHeavyHit(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x *= direction;
        return knockback;
    }
}
