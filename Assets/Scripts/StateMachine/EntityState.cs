using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityState
{
    protected string stateName;
    protected StateMachine stateMachine;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string stateName)
    {
        this.stateMachine = stateMachine;
        this.stateName = stateName;
    }

    public virtual void OnEnter()
    {
        triggerCalled = false;
        anim.SetBool(stateName, true);
    }

    public virtual void OnUpdate()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void OnExit()
    {
        anim.SetBool(stateName, false);
    }

    public virtual void AnimationTrigger() => triggerCalled = true;

    // 设置动画参数
    public virtual void UpdateAnimationParameters() { }
}
