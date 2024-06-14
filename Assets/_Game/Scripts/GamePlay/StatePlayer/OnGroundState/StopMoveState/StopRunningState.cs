using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopRunningState : StopMoveState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        this.OnEndAnim(owner.characterData.stopRunning , owner);
    }
}
