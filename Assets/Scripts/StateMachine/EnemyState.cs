using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    protected Enemy_Health health;
    protected Enemy_VFX vfx;

    public EnemyState(Entity entity, StateMachine stateMachine, string stateName) : base(entity, stateMachine, stateName)
    {
        enemy = (Enemy)entity;
        health = (Enemy_Health)enemy.health;
        vfx = (Enemy_VFX)enemy.vfx;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        float battleAnimSpeedMultiplier = enemy.battleSpeed / enemy.moveSpeed;
        anim.SetFloat("moveAnimSpeedMultiplier", enemy.moveAnimSpeedMultiplier);
        anim.SetFloat("battleAnimSpeedMultiplier", battleAnimSpeedMultiplier);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
