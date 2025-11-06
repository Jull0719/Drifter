using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AttackState : PlayerState
{
    private const int FirstComboIndex = 0; // 默认的第一个攻击动作对应的序号
    private int currentComboIndex;
    private float lastTimeAttacked;
    private int comboLimitedCount = 3; // 组合限制数量

    private float attackDir;
    private float attackTimer;

    private bool comboAttackQueue; // 连击等待
    public Player_AttackState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
        if (comboLimitedCount != player.attackVelocity.Length)
        {
            comboLimitedCount = player.attackVelocity.Length;
            Debug.LogWarning("组合限制数和攻击动作数量不一致，已重置");
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        comboAttackQueue = false;

        ResetComboIndexIfNeeded();
        anim.SetInteger("attackIndex", currentComboIndex);

        // 设置攻击反馈
        attackTimer = player.attackVelocityDuration;
        ApplyVelocity();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        HandleVelocity();

        if (input.Gameplay.Attack.WasPressedThisFrame())
            SetComboAttackQueue();

        if (triggerCalled)
            HandleAttackExit();
    }

    private void SetComboAttackQueue()
    {
        if (currentComboIndex < comboLimitedCount - 1)
            comboAttackQueue = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        lastTimeAttacked = Time.time;
        currentComboIndex++;
    }

    private void HandleAttackExit()
    {
        if (comboAttackQueue)
        {
            anim.SetBool(stateName, false);
            player.EnterComboAttack();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    private void ApplyVelocity()
    {
        // 攻击方向
        attackDir = player.moveInput.x == 0 ? player.facingDir : player.moveInput.x;
        Vector2 velocity = player.attackVelocity[currentComboIndex];
        player.SetVelocity(velocity.x * attackDir, velocity.y);
    }

    private void HandleVelocity()
    {
        attackTimer -= Time.deltaTime; // 攻击反馈计时器
        // 攻击反馈结束
        if (attackTimer < 0)
            player.SetVelocity(0, rb.velocity.y);
    }

    private void ResetComboIndexIfNeeded()
    {
        if (currentComboIndex > comboLimitedCount - 1 || Time.time > lastTimeAttacked + player.comboLimitedTime)
            currentComboIndex = FirstComboIndex;
    }
}

