using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState<T>
{
    public virtual void EnterState(T owner)
    {
        
    }

    public virtual void Execute(T owner)
    {
        
    }

    public virtual void ExitState(T owner)
    {
        
    }
}