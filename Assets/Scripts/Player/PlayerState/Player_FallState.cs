using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_FallState : Player_AiredState
{
    public Player_FallState(Player player, string stateName) : base(player, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (groundSensor.grounded)
            player.ChangeState(player.idleState);
    }
}
