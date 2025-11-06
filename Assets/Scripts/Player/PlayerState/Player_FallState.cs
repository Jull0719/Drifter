using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.groundDetected)
            stateMachine.ChangeState(player.idleState);
    }
}
