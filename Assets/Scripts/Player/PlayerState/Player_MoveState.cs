using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    public Player_MoveState(Player player, string stateName) : base(player, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.velocity.y);

        if (player.moveInput.x == 0)
            player.ChangeState(player.idleState);
    }
}
