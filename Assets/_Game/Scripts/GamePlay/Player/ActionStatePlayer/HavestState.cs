using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HavestState : FarmState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.harvest).Events.OnEnd = () => OnEndEvent(owner);
    }
}
