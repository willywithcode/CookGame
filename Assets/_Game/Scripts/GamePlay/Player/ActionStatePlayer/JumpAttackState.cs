using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class JumpAttackState : BaseState<Player>
{
    protected AnimancerState state;
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        state = owner.characterAnim.PlayBase(owner.characterData.jumpAttack);
        state.Events.OnEnd = () =>
        {
            owner.stateMachine.ChangeState(owner.stateMachine.idleState);
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
        };
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (state.IsStopped)
        {
            owner.stateMachine.ChangeState(owner.stateMachine.idleState);
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
        }
    }
}
