public class Player_CounterAttackState : PlayerState
{
    private bool counterAttackPerformed;
    private Player_Combat combat;
    public Player_CounterAttackState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        combat = player.combat;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        counterAttackPerformed = combat.PerformCounterAttack();
        anim.SetBool("counterAttackPerformed", counterAttackPerformed);
        stateTimer = combat.GetCounterRecoveryTime();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        // 反击时保持静止
        player.SetVelocity(0, rb.velocity.y);

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);

        if (stateTimer < 0 && !counterAttackPerformed)
            stateMachine.ChangeState(player.idleState);
    }
}
