using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 跳跃
        if (player.groundDetected && input.Gameplay.Jump.WasPressedThisFrame())
        {
            stateMachine.ChangeState(player.jumpState);
        }

        // 从平台掉下
        if (!player.groundDetected)
            stateMachine.ChangeState(player.fallState);

        // 攻击
        if (input.Gameplay.Attack.WasPressedThisFrame())
            stateMachine.ChangeState(player.attackState);

        // 反击
        if (input.Gameplay.CounterAttack.WasPressedThisFrame())
            stateMachine.ChangeState(player.counterAttackState);
    }
}
