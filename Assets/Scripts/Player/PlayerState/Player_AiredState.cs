using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * player.moveSpeed * player.airMultiplier, rb.velocity.y);
    }
}
