using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class LandState : OnAirState
{
    private AnimancerState stateAnim;
    public override void EnterState(Player owner)
    {
        stateAnim = owner.ChangeAnim(owner.characterData.land);
        stateAnim.Events.OnEnd = () =>
        {
            this.SwitchOnGround(owner);
        };
    }
}
