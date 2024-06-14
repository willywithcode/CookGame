using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallNormal : FallState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.ChangeAnim(owner.characterData.fallNormal);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        this.CheckCanLand(owner, owner.stateMachine.landNormalState);
    }
}
