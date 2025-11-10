using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    public Enemy_DeadState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.Fade();
        stateMachine.SwitchOffStateMachine(false);
    }
}
