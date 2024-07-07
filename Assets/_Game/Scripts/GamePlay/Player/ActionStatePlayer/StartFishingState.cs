using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartFishingState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.startFishing).Events.OnEnd = () =>
        {
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.pullFishState);
        };
    }
}
