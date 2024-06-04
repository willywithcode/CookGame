using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T>
{
    public void EnterState(T owner);
    public void Execute(T owner);
    public void ExitState(T owner);
}