using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class LandState : BaseState<Player>
{
    private AnimancerState stateAnim;
    public override void EnterState(Player owner)
    {
        stateAnim = owner.ChangeAnim(owner.characterData.land);
    }

    public override void Execute(Player owner)
    {
        if (stateAnim.NormalizedTime >= stateAnim.NormalizedEndTime * 0.5f)
        {
            owner.stateMachine.ChangeState(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f
                ? owner.stateMachine.idleState
                : owner.stateMachine.moveState);
        }
    }

    public override void ExitState(Player owner)
    {
        
    }
}
