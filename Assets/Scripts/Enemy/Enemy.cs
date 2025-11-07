using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [Header("移动")]
    public float moveSpeed = 3f;
    public float idleTime = 2f;
    [Range(0, 2)] public float moveAnimSpeedMultiplier = 1;

    [Header("检测玩家")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Transform playerDetectedPoint;
    public float playerDetectedDistance = 8;

    [Header("攻击")]
    public float attackDetectedDistance = 2;
    public float retreatDistance = 1; // 后撤距离
    public Vector2 retreatVelocity = new Vector2(2, 2);

    [Header("战斗")]
    public float battleSpeed = 4f; // 追逐速度
    public float battleDuration = 5f; // 战斗持续时间

    public Player player { get; private set; }

    public Enemy_IdleState idleState { get; private set; }
    public Enemy_MoveState moveState { get; private set; }
    public Enemy_AttackState attackState { get; private set; }
    public Enemy_BattleState battleState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.InitializedState(idleState);
    }

    public override void TakeDamage(float damage, Entity damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        Player player = damageDealer.GetComponent<Player>();

        if (player != null)
        {
            // 如果已处于Battle/Attack状态下，则不再进入
            if (stateMachine.currentState == battleState || stateMachine.currentState == attackState)
                return;

            // 获取Player
            this.player = player;
            stateMachine.ChangeState(battleState);
        }
    }

    // 检测Player
    public bool DetectedPlayer()
    {
        if (playerDetectedPoint == null)
            playerDetectedPoint = transform;

        RaycastHit2D hit = Physics2D.Raycast(playerDetectedPoint.position, Vector3.right * facingDir,
                            playerDetectedDistance, playerLayer | groundLayer);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
            return false;
        else
        {
            // 获取Player
            player = hit.collider.GetComponent<Player>();
            return true;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        if (playerDetectedPoint == null)
            playerDetectedPoint = transform;

        // 检测Player
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerDetectedPoint.position, playerDetectedPoint.position + Vector3.right * facingDir * playerDetectedDistance);

        // 攻击距离
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerDetectedPoint.position, playerDetectedPoint.position + Vector3.right * facingDir * attackDetectedDistance);

        // 撤退
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(playerDetectedPoint.position, playerDetectedPoint.position + Vector3.right * facingDir * retreatDistance);
    }
}
