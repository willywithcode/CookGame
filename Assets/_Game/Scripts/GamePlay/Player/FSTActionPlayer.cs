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
    private void Start()
    {
        if(owner.CheckHaveDataItem()) ChangeState(holdState);
        else ChangeState(noActionUpperState);
    }
}
