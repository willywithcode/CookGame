using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class LandState : OnAirState
{
    public void OnEndLand(Player owner, ClipTransition clipTransition)
    {
        owner.characterAnim.PlayBase(clipTransition).Events.OnEnd = () =>
        {
            this.SwitchOnGround(owner);
        };
    }
}
