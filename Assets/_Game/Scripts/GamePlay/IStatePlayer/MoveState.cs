using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(Constant.ANIM_WALK_STRING);
        owner.SetupSpeed(TypeOfMove.Walk);
    }

    public void Execute(Player owner)
    {
        owner.MoveInMoveState();
        if(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f) 
        {
            owner.ChangeState(owner.idleState);
        } 
    }

    public void ExitState(Player owner)
    {
        
    }

}
