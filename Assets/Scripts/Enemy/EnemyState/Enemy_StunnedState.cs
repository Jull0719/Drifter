using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_StunnedState : EnemyState
{
    private Enemy_VFX vfx;

    public Enemy_StunnedState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
        vfx = enemy.vfx;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        // 晕眩状态下不能够被反击
        enemy.EnabledCounterableWindow(false);
        //vfx.EnabledAttackAlert(false);

        vfx.EnabledStunnedVfx(true); // 显示晕眩VFX

        stateTimer = enemy.stunnedDuration;
        rb.velocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
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
        vfx.EnabledStunnedVfx(false);
        //vfx.EnabledAttackAlert(false);
    }
}
