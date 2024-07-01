using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : OnGroundState
{
    public override void EnterState(Player owner)
    {
        if (owner.stateMachine.ComparePreviousState(owner.stateMachine.landRollingState) 
            || owner.actionStateMachine.ComparePreviousState(owner.actionStateMachine.punchState)
            || owner.actionStateMachine.ComparePreviousState(owner.actionStateMachine.kickState)
            || owner.stateMachine.CompareCurrentState(owner.stateMachine.fallRollingState))
        {
            owner.characterAnim.PlayBase(owner.characterData.idle_3, false);
            return;
        }
        int random = Random.Range(0, 2);
        if(random == 0) owner.characterAnim.PlayBase(owner.characterData.idle_1, false);
        else owner.characterAnim.PlayBase(owner.characterData.idle_2, false);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (!owner.stateMachine.CompareCurrentState(this)) return;
        owner.character.SetMovementDirection(Vector3.zero);
        if(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) > 0.1f) 
        {
            owner.stateMachine.ChangeState(owner.stateMachine.moveState);
            return;
        }
    }
}
