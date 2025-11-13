using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_IdleState : Player_GroundedState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
    }
}
