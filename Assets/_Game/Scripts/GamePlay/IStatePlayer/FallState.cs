using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.StartFall();
    }

    public void Execute(Player owner)
    {
        owner.Fall();
        owner.Rotate();
    }

    public void ExitState(Player owner)
    {
    }
}
