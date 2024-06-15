using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class JumpRolling : JumpState
{
    public override void EnterState(Player owner)
    {
        countTime = 0;
        owner.character.Jump();
        DOVirtual.DelayedCall(0.2f, () => owner.character.StopJumping());
        this.OnEndJump(owner, owner.characterData.jumpRolling, owner.stateMachine.fallRollingState);
    }
}
