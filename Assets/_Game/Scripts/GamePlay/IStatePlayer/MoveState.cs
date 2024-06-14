using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        if (InputManager.Instance.IsSprint())
        {
            owner.ChangeAnim(owner.characterData.run);
            owner.ChangeSpeed(owner.characterData.runSpeed);
        }
        else
        {
            if (owner.GetMoveDirection().magnitude >= 0.7f)
            {
                owner.ChangeAnim(owner.characterData.jog);
                owner.ChangeSpeed(owner.characterData.jogSpeed);
            }
            else
            {
                owner.ChangeAnim(owner.characterData.walk);
                owner.ChangeSpeed(owner.characterData.walkSpeed);
            }
        }
    }

    public override void Execute(Player owner)
    {
        owner.Rotate();
        owner.character.SetMovementDirection(owner.GetDirectionLocal().Item1);
        this.CheckTypeOfMove(owner);
        if(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f) 
        {
            owner.stateMachine.ChangeState(InputManager.Instance.IsSprint() ? owner.stateMachine.stopMoveState : owner.stateMachine.idleState);
            return;
        }
        if (!owner.character.IsGrounded())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.fallState);
            return;
        }
        if(InputManager.Instance.IsJump())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.jumpState);
            return;
        }
    }

    public override void ExitState(Player owner)
    {
        
    }

    public void CheckTypeOfMove(Player owner)
    {
        if (InputManager.Instance.IsSprint())
        {
            owner.ChangeAnim(owner.characterData.run, 0.5f);
            owner.ChangeSpeed(owner.characterData.runSpeed);
        }
        else
        {
            if (owner.GetMoveDirection().magnitude >= 0.7f)
            {
                owner.ChangeAnim(owner.characterData.jog, 0.5f);
                owner.ChangeSpeed(owner.characterData.jogSpeed);
            }
            else
            {
                owner.ChangeAnim(owner.characterData.walk, 0.5f);
                owner.ChangeSpeed(owner.characterData.walkSpeed);
            }
        }
    }

}
