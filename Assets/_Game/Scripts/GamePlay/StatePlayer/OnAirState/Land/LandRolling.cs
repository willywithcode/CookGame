using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandRolling : LandState
{
    public override void EnterState(Player owner)
    {
        this.OnEndLand(owner, owner.characterData.landRolling);
    }
}
