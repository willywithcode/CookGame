using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpRolling : JumpState
{
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        this.OnEndJump(owner, owner.characterData.jumpRolling, owner.stateMachine.fallRollingState);
    }
}
