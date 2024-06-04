using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameUnit : ACacheMonoBehauviour
{
    public PoolType poolType;

    public virtual void OnInit()
    {
        
    }

    public virtual void OnDespawn()
    {
        
    }
}