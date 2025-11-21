using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : EnemyState
{
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
        HandleDeath();
    }

    private void HandleDeath()
    {
        vfx.Fade(health.GetEnemyDeadTime(), 0);
        stateMachine.SwitchOffStateMachine(false);
    }
}
