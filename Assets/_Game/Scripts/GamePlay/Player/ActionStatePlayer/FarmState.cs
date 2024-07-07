using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FarmState : BaseState<Player>
{
    protected bool isAssignEvent = false;
    public UnityAction onEnd;
    public UnityAction onNotEnd;
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.stateMachine.ChangeState(owner.stateMachine.idleState);
    }
    public void OnEndEvent(Player owner)
    {
        owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
        owner.characterAnim.PlayBase(owner.characterData.idle_1);
        onEnd?.Invoke();
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (!owner.stateMachine.CompareCurrentState(owner.stateMachine.idleState))
        {
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
        }
    }

    public override void ExitState(Player owner)
    {
        base.ExitState(owner);
        onNotEnd?.Invoke();
    }

    public void AssignEvent(UnityAction action)
    {
        onEnd = action;
    }
}
