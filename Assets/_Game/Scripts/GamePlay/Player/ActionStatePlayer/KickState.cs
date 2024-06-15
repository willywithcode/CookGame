using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class KickState : BaseState<Player>
{
    AnimancerState state;
    public override void EnterState(Player owner)
    {
        int kickIndex = Random.Range(0, 3);
        if(kickIndex == 0) state = owner.characterAnim.PlayBase(owner.characterData.kickAttack_1);
        else if(kickIndex == 1) state = owner.characterAnim.PlayBase(owner.characterData.kickAttack_2);
        else state = owner.characterAnim.PlayBase(owner.characterData.kickAttack_3);
        state.Events.OnEnd = () =>
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
