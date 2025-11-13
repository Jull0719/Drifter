using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        if (!enemy.groundDetected || enemy.wallDetected)
            enemy.Flip();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.velocity.y);

        if (!enemy.groundDetected || enemy.wallDetected)
            stateMachine.ChangeState(enemy.idleState);
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
