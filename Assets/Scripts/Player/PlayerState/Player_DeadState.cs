using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();

        input.Disable();
        rb.simulated = false;
        stateMachine.SwitchOffStateMachine(false);
    }
}
