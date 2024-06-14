using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.idle);
    }

    public void Execute(Player owner)
    {
        owner.character.SetMovementDirection(Vector3.zero);
        if (Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) >= 0.1f)
        {
            owner.ChangeState(owner.moveState);
            return;
        }
        if(!owner.character.IsGrounded())
        {
            owner.ChangeState(owner.fallState);
            return;
        }

        if (InputManager.Instance.IsJump())
        {
            owner.ChangeState(owner.jumpState);
            return;
        }
    }

    public void ExitState(Player owner)
    {
        
    }

}
