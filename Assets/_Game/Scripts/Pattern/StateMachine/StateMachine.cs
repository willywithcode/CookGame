using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StateMachine<T> : MonoBehaviour where T : class
{
    [SerializeField] private T owner;
    private BaseState<T> previousBaseState;
    private BaseState<T> currentBaseState;

    private void Update()
    {
        ExecuteState();
    }

    public void ChangeState(BaseState<T> newBaseState)
    {
        if (currentBaseState != null)
        {
            previousBaseState = currentBaseState;
            currentBaseState.ExitState(owner);
        }
        currentBaseState = newBaseState;
        currentBaseState.EnterState(owner);
    }
    public void RevertToPreviousState()
    {
        ChangeState(previousBaseState);
    }
    public void ExecuteState()
    {
        if (currentBaseState != null)
        {
            currentBaseState.Execute(owner);
        }
    }
    public bool CompareCurrentState(BaseState<T> baseState)
    {
        return currentBaseState == baseState;
    }
    public bool ComparePreviousState(BaseState<T> baseState)
    {
        return previousBaseState == baseState;
    }
    
}