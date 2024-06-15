using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSTActionPlayer : StateMachine<Player>
{
    public NoActionUpperState noActionUpperState = new NoActionUpperState();
    public PunchState punchState = new PunchState();
    public KickState kickState = new KickState();
    private void Start()
    {
        this.ChangeState(noActionUpperState);
    }
}
