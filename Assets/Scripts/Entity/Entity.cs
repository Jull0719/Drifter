using System;
using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;

    [SerializeField] protected bool facingRight = true;
    public int facingDir = 1;

    [Header("撤退")]
    [SerializeField] protected bool isKnockback;
    protected Coroutine knockbackCo;

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

    public Entity_Stats stats { get; private set; }

    public StateMachine stateMachine { get; private set; }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        col = GetComponent<Collider2D>();

        stats = GetComponent<Entity_Stats>();

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

    // 击退
    public void ReceiveKnockback(Vector2 knockbackPower, float knockbackDuration)
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
        OnFlipped?.Invoke();
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
    }
}
