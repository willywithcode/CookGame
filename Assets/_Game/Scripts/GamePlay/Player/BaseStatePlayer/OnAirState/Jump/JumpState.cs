using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class JumpState : OnAirState
{
    protected float timeOffset = .5f;
    protected float countTime = 0;
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.stateMachine.ChangeState(owner.stateMachine.ComparePreviousState(owner.stateMachine.runState) ?
            owner.stateMachine.jumpRollingState : owner.stateMachine.jumpNormalState);
    }

    public override void Execute(Player owner)
    {
        this.CheckCanLand(owner);
    }

    public override void ExitState(Player owner)
    {
        countTime = 0;
    }
    public void OnEndJump(Player owner, ClipTransition clipTransition, FallState fallState)
    {
        owner.characterAnim.PlayBase(clipTransition, true).Events.OnEnd = () =>
        {
            owner.stateMachine.ChangeState(fallState);
        };
    }

    public void CheckCanLand(Player owner)
    {
        countTime += Time.deltaTime;
        if(countTime <= timeOffset) return;
        if(owner.character.IsOnGround()) this.SwitchOnGround(owner);
    }
}
