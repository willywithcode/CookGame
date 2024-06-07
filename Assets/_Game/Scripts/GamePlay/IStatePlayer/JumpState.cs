using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(Constant.ANIM_JUMP_STRING);
        owner.SetupSpeed(TypeOfMove.Jump);
        owner.AddInitationVelocity();
    }

    public void Execute(Player owner)
    {
        owner.Jump();
        owner.Rotate();
        owner.Move();
    }

    public void ExitState(Player owner)
    {
    }

    
}
