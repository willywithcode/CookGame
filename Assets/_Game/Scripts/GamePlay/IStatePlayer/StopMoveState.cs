using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class StopMoveState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.stopRunning);
        DOVirtual.DelayedCall(0.5f, () => owner.stateMachine.ChangeState(owner.stateMachine.idleState));
    }

    public override void Execute(Player owner)
    {
        
    }
    
    public override void ExitState(Player owner)
    {
        
    }
}

