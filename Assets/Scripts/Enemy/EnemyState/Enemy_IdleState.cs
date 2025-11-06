using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateTimer = enemy.idleTime;

        enemy.SetVelocity(0, 0);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
