using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNormal : FallState
{
    protected bool canAttack;
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.fallNormal, false);
        canAttack = false;
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if(InputManager.Instance.IsAttack())canAttack = true;
        if(owner.actionStateMachine.CompareCurrentState(owner.actionStateMachine.jumpAttackState) ) return;
        if (owner.character.IsGrounded())
        {
            if (canAttack && highestHeightFall >= owner.characterData.heightEnoughForLanding && 
                !owner.actionStateMachine.CompareCurrentState(owner.actionStateMachine.holdState))
            {
                owner.actionStateMachine.ChangeState(owner.actionStateMachine.jumpAttackState);
            }
            else this.CheckCanLand(owner, owner.stateMachine.landNormalState);
        }
        
    }
}
