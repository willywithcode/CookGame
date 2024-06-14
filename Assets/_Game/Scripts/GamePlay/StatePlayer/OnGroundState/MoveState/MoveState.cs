using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : OnGroundState
{
    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if(!owner.stateMachine.CompareCurrentState(this)) return;
        owner.Rotate();
        owner.character.SetMovementDirection(owner.GetCurrentDirectionLocal().Item1);
        this.CheckTypeOfMove(owner);
    }
    public void CheckTypeOfMove(Player owner)
    {
        if (InputManager.Instance.IsSprint())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.runState);
            return;
        }
        owner.stateMachine.ChangeState(InputManager.Instance.GetMoveDirection().magnitude >= 0.7f ? 
            owner.stateMachine.jogState :  owner.stateMachine.walkState );
    }
}
