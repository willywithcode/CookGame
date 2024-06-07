using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipState : IState<Player>
{
    public void EnterState(Player owner)
    {
        Debug.Log("enter slip state");
    }

    public void Execute(Player owner)
    {
        Debug.Log("slipping");
        owner.Slip();
        owner.Rotate();
    }

    public void ExitState(Player owner)
    {
    }
}
