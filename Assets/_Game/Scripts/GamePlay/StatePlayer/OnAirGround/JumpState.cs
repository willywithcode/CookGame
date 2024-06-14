using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class JumpState : OnAirState
{
    private float timeOffset = 0.1f;
    private float countTime = 0;
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        DOVirtual.DelayedCall(0.2f, () => owner.character.StopJumping());
        owner.ChangeAnim(owner.characterData.jump).Events.OnEnd = () =>
        {
            owner.stateMachine.ChangeState(owner.stateMachine.fallState);
        };
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
}
