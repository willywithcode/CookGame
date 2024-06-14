using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : IState<Player>
{
    public void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.fall);
    }

    public void Execute(Player owner)
    {
        owner.character.SetMovementDirection(Vector3.zero);
        if (owner.character.IsGrounded())
        {
            owner.ChangeState(owner.landState);
        }
    }

    public void ExitState(Player owner)
    {
        
    }
}
