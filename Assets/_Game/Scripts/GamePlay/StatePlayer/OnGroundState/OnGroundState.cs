using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundState : BaseState<Player>
{
    public override void Execute(Player owner)
    {
        if (!owner.character.IsGrounded())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.fallNormalState);
            return;
        }
        if(InputManager.Instance.IsJump())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.jumpState);
            return;
        }
    }
}
