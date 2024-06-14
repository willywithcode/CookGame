using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class JumpState : OnAirState
{
    protected float timeOffset = 0.1f;
    protected float countTime = 0;
    public override void EnterState(Player owner)
    {
        if (owner.stateMachine.ComparePreviousState(owner.stateMachine.runState))
        {
            owner.stateMachine.ChangeState(owner.stateMachine.jumpRollingState);
        }
        else
        {
            owner.stateMachine.ChangeState(owner.stateMachine.jumpNormalState);
        }
    }

    public override void Execute(Player owner)
    {
        countTime += Time.deltaTime;
        if(countTime <= timeOffset) return;
        if(owner.character.IsOnGround()) this.SwitchOnGround(owner);
    }
    public override void ExitState(Player owner)
    {
        countTime = 0;
    }
    public void OnEndJump(Player owner, ClipTransition clipTransition, FallState fallState)
    {
        owner.ChangeAnim(clipTransition).Events.OnEnd = () =>
        {
            owner.stateMachine.ChangeState(fallState);
        };
    }
}
