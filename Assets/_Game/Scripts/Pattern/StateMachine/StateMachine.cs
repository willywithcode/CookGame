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
        if(!GameManager.CompareCurrentState(GameState.Gameplay))
            return;
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
        if(currentBaseState == null) return false;
        return baseState.GetType() == currentBaseState.GetType();
    }
    public bool ComparePreviousState(BaseState<T> baseState)
    {
        if(previousBaseState == null) return false;
        return previousBaseState.GetType() == baseState.GetType();
    }
    public bool CompareCurrentStateIsOrTypeOf(BaseState<T> baseState)
    {
        if (currentBaseState == null) return false;
        return baseState.GetType() == currentBaseState.GetType() || currentBaseState.GetType().IsSubclassOf(baseState.GetType());
    }
    public bool ComparePreviousStateIsOrTypeOf(BaseState<T> baseState)
    {
        if(previousBaseState == null) return false;
        return previousBaseState.GetType() == baseState.GetType() || previousBaseState.GetType().IsSubclassOf(baseState.GetType());
    }
    
}