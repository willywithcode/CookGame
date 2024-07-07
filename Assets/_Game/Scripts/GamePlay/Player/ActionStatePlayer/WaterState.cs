using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterState : FarmState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.water).Events.OnEnd = () => OnEndEvent(owner);
    }
}
