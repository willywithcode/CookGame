using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(Constant.ANIM_IDLE_STRING);
    }

    public void Execute(Player owner)
    {
        if (Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) >= 0.1f)
        {
            owner.ChangeState(owner.moveState);
        }
    }

    public void ExitState(Player owner)
    {
        
    }

}
