using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StopMoveState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.stopRunning);
        DOVirtual.DelayedCall(0.5f, () => owner.ChangeState(owner.idleState));
    }

    public void Execute(Player owner)
    {
        
    }
    
    public void ExitState(Player owner)
    {
        
    }
}
