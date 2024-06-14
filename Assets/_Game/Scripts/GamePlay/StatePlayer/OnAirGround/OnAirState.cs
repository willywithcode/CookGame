using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAirState : BaseState<Player>
{
    public void SwitchOnGround(Player owner)
    {
        owner.stateMachine.ChangeState(Vector3.Distance(InputManager.Instance.GetMoveDirection(), Vector3.zero) <= 0.1f
            ? owner.stateMachine.idleState
            : owner.stateMachine.moveState);
        return;
    }
}
