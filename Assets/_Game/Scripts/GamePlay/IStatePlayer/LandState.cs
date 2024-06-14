using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class LandState : IState<Player>
{
    private AnimancerState stateAnim;
    public void EnterState(Player owner)
    {
        stateAnim = owner.ChangeAnim(owner.characterData.land);
    }

    public void Execute(Player owner)
    {
        if (stateAnim.NormalizedTime >= stateAnim.NormalizedEndTime * 0.5f)
        {
            owner.ChangeState(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f
                ? owner.idleState
                : owner.moveState);
        }
    }

    public void ExitState(Player owner)
    {
        
    }
}
