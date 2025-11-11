using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StunState : EnemyState
{
    public Enemy_StunState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        enemy.SetVelocity(enemy.stunnedVelocity.x * enemy.facingDir, enemy.stunnedVelocity.y);
        vfx.EnabledCounterSign(true);
        stateTimer = enemy.stunnedDuration;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit();
        vfx.EnabledCounterSign(false);
    }
}
