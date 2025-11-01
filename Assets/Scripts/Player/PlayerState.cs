using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected string stateName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Sensor_Ground groundSensor;
    protected PlayerInput input;

    protected bool triggerCalled;
    public PlayerState(Player player, string stateName)
    {
        this.player = player;
        this.stateName = stateName;

        anim = player.anim;
        rb = player.rb;
        groundSensor = player.groundSensor;
        input = player.input;
    }

    public virtual void OnEnter()
    {
        triggerCalled = false;
        anim.SetBool(stateName, true);
    }

    public virtual void OnUpdate()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    public virtual void OnExit()
    {
        anim.SetBool(stateName, false);
    }

    public virtual void CallAnimationTrigger() => triggerCalled = true;
}
