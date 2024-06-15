using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNormal : FallState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.fallNormal, false);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        this.CheckCanLand(owner, owner.stateMachine.landNormalState);
    }
}
