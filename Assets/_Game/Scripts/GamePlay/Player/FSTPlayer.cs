using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FSTPlayer : StateMachine<Player>
{
    public OnGroundState onGroundState = new OnGroundState();
    public OnAirState onAirState = new OnAirState();
    public IdleState idleState = new IdleState();
    public MoveState moveState = new MoveState();
    public JumpState jumpState = new JumpState();
    public JumpNormal jumpNormalState = new JumpNormal();
    public JumpRolling jumpRollingState = new JumpRolling();
    public FallNormal fallNormalState = new FallNormal();
    public FallRolling fallRollingState = new FallRolling();
    public StopMoveState stopMoveState = new StopMoveState();
    public StopRunningState stopRunningState = new StopRunningState();
    public StopWalkingState stopWalkingState = new StopWalkingState();
    public StopJoggingState stopJoggingState = new StopJoggingState();
    public LandNormal landNormalState = new LandNormal();
    public LandRolling landRollingState = new LandRolling();
    public RunState runState = new RunState();
    public WalkState walkState = new WalkState();
    public JogState jogState = new JogState();

    private void Start()
    {
        this.ChangeState(idleState);
    }
}
