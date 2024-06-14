using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : IState<Player>
{
    public void EnterState(Player owner)
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

    public void Execute(Player owner)
    {
        owner.Rotate();
        owner.character.SetMovementDirection(owner.GetDirectionLocal().Item1);
        this.CheckTypeOfMove(owner);
        if(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f) 
        {
            owner.ChangeState(InputManager.Instance.IsSprint() ? owner.stopMoveState : owner.idleState);
            return;
        }
        if (!owner.character.IsGrounded())
        {
            owner.ChangeState(owner.fallState);
            return;
        }
        if(InputManager.Instance.IsJump())
        {
            owner.ChangeState(owner.jumpState);
            return;
        }
    }

    public void ExitState(Player owner)
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
