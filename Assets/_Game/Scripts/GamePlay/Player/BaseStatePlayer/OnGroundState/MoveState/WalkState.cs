using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MoveState
{
    public override void EnterState(Player owner)
    {
        owner.characterAnim.PlayBase(owner.characterData.walk);
        owner.ChangeSpeed(owner.characterData.walkSpeed);
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
