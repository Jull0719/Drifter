using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Entity : MonoBehaviour
{
    [SerializeField] protected bool facingRight = true;
    public int facingDir = 1;

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

    // 检测碰撞
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
        if (groundDetectedPoint == null)
            groundDetectedPoint = transform;

        Gizmos.DrawLine(groundDetectedPoint.position, groundDetectedPoint.position + Vector3.down * groundDetectedDistance);
        Gizmos.DrawLine(firstWallDetectedPoint.position, firstWallDetectedPoint.position + Vector3.right * facingDir * wallDetectedDistance);
        if (secondWallDetectedPoint)
            Gizmos.DrawLine(secondWallDetectedPoint.position, secondWallDetectedPoint.position + Vector3.right * facingDir * wallDetectedDistance);
    }
}
