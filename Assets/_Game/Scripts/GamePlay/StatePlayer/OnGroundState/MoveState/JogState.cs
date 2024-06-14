using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JogState : MoveState
{
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.jog);
        owner.ChangeSpeed(owner.characterData.jogSpeed);
    }

    public override void Execute(Player owner)
    {
        if(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) <= 0.1f) 
        {
            owner.stateMachine.ChangeState(owner.stateMachine.idleState);
            return;
        }
        base.Execute(owner);
        
    }
}
