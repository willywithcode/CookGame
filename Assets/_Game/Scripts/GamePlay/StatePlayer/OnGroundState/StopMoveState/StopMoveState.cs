using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class StopMoveState : OnGroundState
{
    protected Vector3 moveDirection;
    public override void EnterState(Player owner)
    {
        moveDirection = owner.GetDirectionLocal(InputManager.Instance.GetPreviousMoveDirection()).Item1;
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if(!owner.stateMachine.CompareCurrentState(this)) return;
        if(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) > 0.1f) 
        {
            owner.stateMachine.ChangeState(owner.stateMachine.moveState);
            return;
        }
        owner.character.SetMovementDirection(moveDirection);
    }

    public void OnEndAnim(ClipTransition clip, Player owner)
    {
        owner.ChangeAnim(owner.characterData.stopRunning).Events.OnEnd =
            () =>
            {
                owner.stateMachine.ChangeState(owner.stateMachine.idleState);
            };
    }
}

