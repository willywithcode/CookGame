using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class JumpState : IState<Player>
{
    private float time = 0.1f;
    private float countTime = 0;
    private AnimancerState stateAnim;
    public void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        stateAnim = owner.ChangeAnim(owner.characterData.jump);
    }

    public void Execute(Player owner)
    {
        if(stateAnim.NormalizedTime >= stateAnim.NormalizedEndTime * 0.5f)
        {
            owner.character.StopJumping();
            owner.ChangeState(owner.fallState);
            return;
        }
        countTime += Time.deltaTime;
        if(countTime <= time) return;
        if (owner.character.IsGrounded())
        {
            owner.character.StopJumping();
            owner.ChangeState(Vector3.Distance(owner.GetMoveDirection(), Vector3.zero) <= 0.1f
                ? owner.idleState
                : owner.moveState);
            return;
        }
        
    }

    public void ExitState(Player owner)
    {
        countTime = 0;
    }

    
}
