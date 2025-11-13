using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BattleState : EnemyState
{
    private float lastTimeWasInBattle;
    private Player player;

    public Enemy_BattleState(Enemy enemy, StateMachine stateMachine, string stateName) : base(enemy, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        UpdateBattleTimer();

        if (player == null)
            player = enemy.player;

        if (ShouldRetreat())
        {
            rb.velocity = new Vector2(enemy.retreatVelocity.x * -DirectionToPlayer(), enemy.retreatVelocity.y);
            enemy.HandleFlipped(DirectionToPlayer());
        }
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (enemy.DetectedPlayer())
            UpdateBattleTimer();

        if (BattleIsOver())
            stateMachine.ChangeState(enemy.idleState);

        if (WithAttackRange() && enemy.DetectedPlayer())
            stateMachine.ChangeState(enemy.attackState);
        else
            enemy.SetVelocity(enemy.battleSpeed * DirectionToPlayer(), rb.velocity.y);
    }

    private void UpdateBattleTimer() => lastTimeWasInBattle = Time.time;
    private bool BattleIsOver() => Time.time > lastTimeWasInBattle + enemy.battleDuration;
    private bool WithAttackRange() => DistanceToPlayer() < enemy.attackDetectedDistance;
    private bool ShouldRetreat() => DistanceToPlayer() < enemy.retreatDistance;
    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(enemy.transform.position.x - player.transform.position.x);
    }
    private int DirectionToPlayer()
    {
        if (player == null)
            return 0;

        float horizontalDistance = Mathf.Abs(enemy.transform.position.x - player.transform.position.x);
        float verticalDistance = Mathf.Abs(enemy.transform.position.y - player.transform.position.y);

        if (horizontalDistance < 1f && verticalDistance > 1.5f)
            return 0;

        return player.transform.position.x < enemy.transform.position.x ? -1 : 1;
    }
}