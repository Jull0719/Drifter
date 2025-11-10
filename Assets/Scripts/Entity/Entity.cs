using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected bool facingRight = true;
    public int facingDir = 1;


    [Header("撤退")]
    [SerializeField] protected bool isKnockback;
    [SerializeField] protected Vector2 knockbackPower = new Vector2(3, 2);
    [SerializeField] protected float knockbackDuration = 0.15f;
    [Header("重击")]
    [SerializeField] protected float heavyThresholder = 0.3f; // 伤害占最大生命值的30%
    [SerializeField] protected Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] protected float heavyKnockbackDuration = 0.2f;
    protected Coroutine knockbackCo;
    [Space]

    [Header("VFX")]
    [SerializeField] protected Material damageVfxMaterial;
    [SerializeField] protected float damageVfxDuration = 0.3f;
    private Material defaultMaterial;
    protected Coroutine damageVfxCo;
    [Space]

    [Header("攻击检测")]
    [SerializeField] protected float attackRadius = 1;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;
    [Space]

    [Header("血量设置")]
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;
    protected bool isDead;
    [SerializeField] protected float damage = 10;
    [Space]

    [Header("碰撞检测")]
    public bool groundDetected;
    public LayerMask groundLayer;
    [SerializeField] protected Transform groundDetectedPoint;
    [SerializeField] protected float groundDetectedDistance = 1f;
    public bool wallDetected;
    [SerializeField] protected Transform firstWallDetectedPoint;
    [SerializeField] protected Transform secondWallDetectedPoint;
    [SerializeField] protected float wallDetectedDistance = 1f;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Collider2D col { get; private set; }
    public StateMachine stateMachine { get; private set; }

    protected SpriteRenderer sr;

    protected Coroutine fadeCo;

    protected virtual void Awake()
    {
        stateMachine = new StateMachine();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();

        sr = GetComponentInChildren<SpriteRenderer>();

        defaultMaterial = sr.material;

        currentHealth = maxHealth;
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetected();
        stateMachine.UpdateActiveState();
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
        ReceiveKnockback(knockback, duration);

        OnDamageVfx();
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

    // 击退
    private void ReceiveKnockback(Vector2 knockbackPower, float knockbackDuration)
    {
        if (knockbackCo != null)
            StopCoroutine(knockbackCo);

        knockbackCo = StartCoroutine(KnockbackCo(knockbackPower, knockbackDuration));
    }

    IEnumerator KnockbackCo(Vector2 knockbackPower, float knockbackDuration)
    {
        isKnockback = true;
        rb.velocity = knockbackPower;
        yield return new WaitForSeconds(knockbackDuration);
        rb.velocity = Vector2.zero;
        isKnockback = false;
    }

    // 受击效果
    public void OnDamageVfx()
    {
        if (damageVfxCo != null)
            StopCoroutine(damageVfxCo);

        damageVfxCo = StartCoroutine(OnDamageVfxCo());
    }

    IEnumerator OnDamageVfxCo()
    {
        sr.material = damageVfxMaterial;
        yield return new WaitForSeconds(damageVfxDuration);
        sr.material = defaultMaterial;
    }

    // 实施攻击
    public void PerformAttack()
    {
        foreach (var target in TargetDetected())
            target.GetComponent<Entity>().TakeDamage(damage, this);
    }

    // 攻击检测
    private Collider2D[] TargetDetected()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, attackRadius * facingDir, targetLayer);
    }

    // 状态结束
    public void CurrentStateAnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    // 设置速度
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnockback) return;

        rb.velocity = new Vector2(xVelocity, yVelocity);
        HandleFlipped(xVelocity);
    }

    // 控制人物朝向
    public void HandleFlipped(float xVelocity)
    {
        if (xVelocity > 0 && !facingRight)
            Flip();
        else if (xVelocity < 0 && facingRight)
            Flip();
    }

    public void Flip()
    {
        facingRight = !facingRight;
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    // 检测地面和墙壁碰撞
    private void HandleCollisionDetected()
    {
        groundDetected = Physics2D.Raycast(groundDetectedPoint.position, Vector3.down, groundDetectedDistance, groundLayer);
        wallDetected = Physics2D.Raycast(firstWallDetectedPoint.position, Vector3.right * facingDir, wallDetectedDistance, groundLayer);

        if (secondWallDetectedPoint)
            wallDetected = Physics2D.Raycast(firstWallDetectedPoint.position, Vector3.right * facingDir, wallDetectedDistance, groundLayer)
                        || Physics2D.Raycast(secondWallDetectedPoint.position, Vector3.right * facingDir, wallDetectedDistance, groundLayer);
    }

    protected virtual void OnDrawGizmos()
    {
        // 地面和墙壁
        if (groundDetectedPoint == null)
            groundDetectedPoint = transform;

        Gizmos.DrawLine(groundDetectedPoint.position, groundDetectedPoint.position + Vector3.down * groundDetectedDistance);
        Gizmos.DrawLine(firstWallDetectedPoint.position, firstWallDetectedPoint.position + Vector3.right * facingDir * wallDetectedDistance);
        if (secondWallDetectedPoint)
            Gizmos.DrawLine(secondWallDetectedPoint.position, secondWallDetectedPoint.position + Vector3.right * facingDir * wallDetectedDistance);

        // 攻击
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


    // 逐渐变透明
    public void Fade()
    {
        if (fadeCo != null)
            StopCoroutine(fadeCo);

        fadeCo = StartCoroutine(FadeCo());
    }

    IEnumerator FadeCo()
    {
        Color currentColor = sr.color;
        while (currentColor.a > 0)
        {
            currentColor.a -= Time.deltaTime * 0.3f;
            sr.color = currentColor;
            yield return null;
        }
        sr.color = currentColor;
    }
}
