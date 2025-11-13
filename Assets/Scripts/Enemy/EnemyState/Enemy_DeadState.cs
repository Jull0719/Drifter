using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
    private float deadTime;
    private Enemy_VFX vfx;
    private Enemy_Health health;

    public Enemy_DeadState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
        vfx = enemy.vfx;
        health = enemy.health;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        vfx.Fade(health.GetEnemyDeadTime(), 0);
        stateMachine.SwitchOffStateMachine(false);
    }
}
