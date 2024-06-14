using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : OnGroundState
{
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.idle);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (!owner.stateMachine.CompareCurrentState(this)) return;
        owner.character.SetMovementDirection(Vector3.zero);
        if(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) > 0.1f) 
        {
            owner.stateMachine.ChangeState(owner.stateMachine.moveState);
            return;
        }
    }
}
