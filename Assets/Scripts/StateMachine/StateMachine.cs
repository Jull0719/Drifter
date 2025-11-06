using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    // 当前状态
    public EntityState currentState { get; private set; }

    public void InitializedState(EntityState startState)
    {
        currentState = startState;
        currentState.OnEnter();
    }

    public void UpdateActiveState()
    {
        currentState.OnUpdate();
    }

    public void ChangeState(EntityState newState)
    {
        currentState.OnExit();
        currentState = newState;
        currentState.OnEnter();
    }
}
