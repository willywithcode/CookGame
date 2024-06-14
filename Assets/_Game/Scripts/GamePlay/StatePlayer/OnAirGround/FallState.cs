using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : OnAirState
{
    private bool isHighEnough;
    private float highestHeightFall;
    public override void EnterState(Player owner)
    {
        owner.ChangeAnim(owner.characterData.fall);
        isHighEnough = false;
        highestHeightFall = 0;
    }

    public override void Execute(Player owner)
    {
        owner.character.SetMovementDirection(Vector3.zero);
        isHighEnough = CalculateHeightFall(owner) >= owner.characterData.heightEnoughForLanding;
        if (owner.character.IsGrounded())
        {
            if (isHighEnough)
            {
                owner.stateMachine.ChangeState(owner.stateMachine.landState);
                return;
            }
            this.SwitchOnGround(owner);
        }
    }
    public float CalculateHeightFall(Player owner)
    {
        if(Physics.Raycast(owner.TF.position, Vector3.down, out RaycastHit hit
               , 50, 1 << LayerMask.NameToLayer(Constant.LAYER_GROUND_STRING)))
        {
            if((owner.TF.position.y - hit.point.y) > highestHeightFall)
            {
                highestHeightFall = (owner.TF.position.y - hit.point.y);
            }
        }
        return highestHeightFall;
    }
}
