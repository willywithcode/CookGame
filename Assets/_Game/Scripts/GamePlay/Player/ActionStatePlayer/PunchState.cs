using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class PunchState : BaseState<Player>
{
    AnimancerState state;
    public override void EnterState(Player owner)
    {
        int punch = Random.Range(0, 2);
        ClipTransition punchState = punch == 0 ? owner.characterData.punchLeftAttack : owner.characterData.punchRightAttack;
        state = owner.characterAnim.PlayAction(punchState);
        state.Events.OnEnd += () =>
        {
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
        };
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if(state.IsStopped) owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
    }
}
