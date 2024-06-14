using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSTPlayer : StateMachine<Player>
{
    public IdleState idleState = new IdleState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public StopMoveState stopMoveState = new StopMoveState();
    public LandState landState = new LandState();
}
