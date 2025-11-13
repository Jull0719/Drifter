using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        player.SetVelocity(0, player.jumpForce);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.fallState);
    }
}
