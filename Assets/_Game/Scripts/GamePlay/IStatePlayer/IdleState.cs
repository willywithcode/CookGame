using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.idle);
    }

    public override void Execute(Player owner)
    {
        owner.character.SetMovementDirection(Vector3.zero);
        if (Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) >= 0.1f)
        {
            owner.stateMachine.ChangeState(owner.stateMachine.moveState);
            return;
        }
        if(!owner.character.IsGrounded())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.fallState);
            return;
        }

        if (InputManager.Instance.IsJump())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.jumpState);
            return;
        }
    }

    public override void ExitState(Player owner)
    {
        
    }

}
