using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRolling : FallState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.fallRolling);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        this.CheckCanLand(owner, owner.stateMachine.landRollingState);
    }
}
