using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopWalkingState : StopMoveState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        this.OnEndAnim(owner.characterData.stopWalking , owner);
    }
}
