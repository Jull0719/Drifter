using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (enemy.DetectedPlayer())
            stateMachine.ChangeState(enemy.battleState);
    }
}
