using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpNormal : JumpState
{
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        this.OnEndJump(owner, owner.characterData.jump, owner.stateMachine.fallNormalState);
    }
}
