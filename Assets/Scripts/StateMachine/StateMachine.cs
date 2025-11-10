using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // 当前状态
    public EntityState currentState { get; private set; }
    private bool canChangeState;

    public void InitializedState(EntityState startState)
    {
        canChangeState = true;
        currentState = startState;
        currentState.OnEnter();
    }

    public void UpdateActiveState()
    {
        currentState.OnUpdate();
    }

    public void ChangeState(EntityState newState)
    {
        if (!canChangeState)
            return;

        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }

    public void SwitchOffStateMachine(bool enabled)
    {
        canChangeState = enabled;
    }
}
