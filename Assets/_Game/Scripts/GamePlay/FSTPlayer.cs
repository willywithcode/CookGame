using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSTPlayer : StateMachine<Player>
{
    public IdleState idleState = new IdleState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public FallState fallState = new FallState();
    public StopRunningState stopRunningState = new StopRunningState();
    public StopWalkingState stopWalkingState = new StopWalkingState();
    public StopJoggingState stopJoggingState = new StopJoggingState();
    public LandState landState = new LandState();
    public RunState runState = new RunState();
    public WalkState walkState = new WalkState();
    public JogState jogState = new JogState();
}
