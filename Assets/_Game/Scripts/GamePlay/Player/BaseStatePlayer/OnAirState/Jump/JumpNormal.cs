using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpNormal : JumpState
{
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        DOVirtual.DelayedCall(0.2f, () => owner.character.StopJumping());
        this.OnEndJump(owner, owner.characterData.jump, owner.stateMachine.fallNormalState);
    }
}
