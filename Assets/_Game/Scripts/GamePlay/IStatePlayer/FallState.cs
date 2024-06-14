using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.fall);
    }

    public override void Execute(Player owner)
    {
        owner.character.SetMovementDirection(Vector3.zero);
        if (owner.character.IsGrounded())
        {
            owner.stateMachine.ChangeState(owner.stateMachine.landState);
        }
    }

    public override void ExitState(Player owner)
    {
        
    }
}
