using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditorInternal;
using UnityEngine;

public class Enemy_Health : Entity_Health, IDamagable
{
    [Header("死亡")]
    [SerializeField] protected float deadTime = 3;

    private Enemy enemy;
    private StateMachine stateMachine;

    protected override void Awake()
    {
        base.Awake();
        enemy = entity as Enemy;
    }

    protected override void Start()
    {
        base.Start();

        stateMachine = enemy.stateMachine;
    }

    // 获取敌人死亡时间
    public float GetEnemyDeadTime() => deadTime;

    public override void OnDie()
    {
        base.OnDie();
        stateMachine.ChangeState(enemy.deadState);
        Destroy(gameObject, deadTime);
    }

    // 受击
    public override void TakeDamage(float damage, Entity damageDealer)
    {
        base.TakeDamage(damage, damageDealer);

        //Debug.Log("Enemy - " + gameObject.name + "受到攻击");
        if (isDead) return;

        Player player = damageDealer.GetComponent<Player>();

        if (player != null)
        {
            // 如果已处于Battle/Attack状态下，则不再进入
            if (stateMachine.currentState == enemy.battleState || stateMachine.currentState == enemy.attackState)
                return;

            // 获取Player
            enemy.GetPlayerReference(player);

            stateMachine.ChangeState(enemy.battleState);
        }
    }
}
