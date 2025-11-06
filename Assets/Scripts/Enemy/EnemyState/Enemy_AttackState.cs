using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (triggerCalled)
            stateMachine.ChangeState(enemy.battleState);
    }
}
