using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSTActionPlayer : StateMachine<Player>
{
    public NoActionUpperState noActionUpperState = new NoActionUpperState();
    public PunchState punchState = new PunchState();
    public KickState kickState = new KickState();
    public JumpAttackState jumpAttackState = new JumpAttackState();
    public HoldState holdState = new HoldState();
    public FarmState farmState = new FarmState();
    public PlantState plantState = new PlantState();
    public WaterState waterState = new WaterState();
    public HavestState havestState = new HavestState();
    public StartFishingState startFishingState = new StartFishingState();
    public PullFishState pullFishState = new PullFishState();
    private void Start()
    {
        if(owner.CheckHaveDataItem()) ChangeState(holdState);
        else ChangeState(noActionUpperState);
    }
}
