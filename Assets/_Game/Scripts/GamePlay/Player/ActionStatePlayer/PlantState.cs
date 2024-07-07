using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantState : FarmState
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.plantSeed).Events.OnEnd = () => OnEndEvent(owner);
    }
}
