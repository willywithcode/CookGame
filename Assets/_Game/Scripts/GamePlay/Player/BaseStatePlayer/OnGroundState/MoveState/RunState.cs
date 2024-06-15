using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MoveState
{
    public override void EnterState(Player owner)
    {
        owner.characterAnim.PlayBase(owner.characterData.run);
        owner.ChangeSpeed(owner.characterData.runSpeed);
    }
    public override void Execute(Player owner)
    {
        if(owner.actionStateMachine.CompareCurrentState(owner.actionStateMachine.kickState)) return;
        if(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) <= 0.1f) 
        {
            owner.stateMachine.ChangeState(owner.stateMachine.stopRunningState);
            return;
        }
        base.Execute(owner);
        
    }

}
