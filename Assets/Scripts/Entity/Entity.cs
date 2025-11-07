using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected bool facingRight = true;
    public int facingDir = 1;

    [Header("攻击检测")]
    [SerializeField] protected float attackRadius = 1;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform attackPoint;

    [Header("血量设置")]
    [SerializeField] protected float maxHealth = 100;
    protected float currentHealth;
    protected bool isDead;
    [SerializeField] protected float damage = 10;

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
    public StateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;

        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleCollisionDetected();
        stateMachine.UpdateActiveState();
    }

    // 受到伤害
    public virtual void TakeDamage(float damage, Entity damageDealer)
    {
        if (isDead) return;

        ReduceHealth(damage);
    }

    public void ReduceHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " is Died.");
    }

    // 实施攻击
    public void PerformAttack()
    {
        foreach (var target in TargetDetected())
        {
            Debug.Log(target.name + "受到攻击");
            target.GetComponent<Entity>().TakeDamage(damage, this);
        }
    }

    // 攻击检测
    public Collider2D[] TargetDetected()
    {
        return Physics2D.OverlapCircleAll(attackPoint.position, attackRadius * facingDir, targetLayer);
    }

    // 状态结束
    public void CurrentStateAnimationTrigger() => stateMachine.currentState.AnimationTrigger();

    // 设置速度
    public void SetVelocity(float xVelocity, float yVelocity)
    {
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
}
