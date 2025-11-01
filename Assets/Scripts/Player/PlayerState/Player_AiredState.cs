using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_AiredState : PlayerState
{
    public Player_AiredState(Player player, string stateName) : base(player, stateName)
    {
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (player.moveInput.x != 0)
            player.SetVelocity(player.moveInput.x * player.moveSpeed * player.airMultiplier, rb.velocity.y);
    }
}
