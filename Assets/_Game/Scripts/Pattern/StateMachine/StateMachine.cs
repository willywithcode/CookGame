using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StateMachine<T> where T : class
{
    [SerializeField] private T owner;
    private IState<T> previousState;
    private IState<T> currentState;

    public StateMachine(T owner)
    {
        this.owner = owner;
        currentState = null;
    }
    public void ChangeState(IState<T> newState)
    {
        if (currentState != null)
        {
            previousState = currentState;
            currentState.ExitState(owner);
        }
        currentState = newState;
        currentState.EnterState(owner);
    }
    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }
    public void ExecuteState()
    {
        if (currentState != null)
        {
            currentState.Execute(owner);
        }
    }
    public bool CompareCurrentState(IState<T> state)
    {
        return currentState == state;
    }
    
}