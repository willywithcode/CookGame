using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoActionUpperState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        owner.characterAnim.FadeOutUpperBody();
        UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractItem(false);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (InputManager.Instance.IsAttack() && owner.stateMachine.CompareCurrentStateIsOrTypeOf(owner.stateMachine.onGroundState)
            && !owner.stateMachine.CompareCurrentStateIsOrTypeOf(owner.stateMachine.stopMoveState))
        {
            if (owner.stateMachine.CompareCurrentState(owner.stateMachine.runState))
            {
                owner.actionStateMachine.ChangeState(owner.actionStateMachine.kickState);
                return;
            }
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.punchState);
            return;
        }
    }
    
}
