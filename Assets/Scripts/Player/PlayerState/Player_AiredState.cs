using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 空中移动
        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * player.moveSpeed * player.airMultiplier, rb.velocity.y);

        // 二段跳
        if (input.Gameplay.Jump.WasPressedThisFrame() && player.canDoubleJump)
        {
            player.EnabledDoubleJump(false);
            stateMachine.ChangeState(player.jumpState);
        }
    }
}
