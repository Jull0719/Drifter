using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private float deadTime;
    public Enemy_DeadState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
        deadTime = health.GetEnemyDeadTime();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        vfx.Fade(deadTime, 0);
        stateMachine.SwitchOffStateMachine(false);
    }
}
