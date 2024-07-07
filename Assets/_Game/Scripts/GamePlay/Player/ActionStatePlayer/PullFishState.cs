using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullFishState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        owner.characterAnim.PlayBase(owner.characterData.pullFish);
    }
}
