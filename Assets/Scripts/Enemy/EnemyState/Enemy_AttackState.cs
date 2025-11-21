using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        SyncAttackSpeed();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
