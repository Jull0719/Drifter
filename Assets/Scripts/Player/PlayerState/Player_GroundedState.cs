using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GroundedState : PlayerState
{
    public Player_GroundedState(Player player, string stateName) : base(player, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 跳跃
        if (groundSensor.grounded && input.Gameplay.Jump.WasPressedThisFrame())
        {
            player.ChangeState(player.jumpState);
        }

        // 从平台掉下
        if (!groundSensor.grounded)
            player.ChangeState(player.fallState);

        // 攻击
        if (input.Gameplay.Attack.WasPressedThisFrame())
            player.ChangeState(player.attackState);
    }
}
