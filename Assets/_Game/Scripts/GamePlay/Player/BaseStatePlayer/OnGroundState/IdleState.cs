using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : OnGroundState
{
    public override void EnterState(Player owner)
    {
        if (owner.stateMachine.ComparePreviousState(owner.stateMachine.landRollingState))
        {
            owner.characterAnim.PlayBase(owner.characterData.idle_3, true);
            return;
        }
        int random = Random.Range(0, 2);
        if(random == 0) owner.characterAnim.PlayBase(owner.characterData.idle_1, true);
        else owner.characterAnim.PlayBase(owner.characterData.idle_2, true);
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
