using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : BaseState<Player>
{
    public override void EnterState(Player owner)
    {
        base.EnterState(owner);
        if(owner.ListItemHold.Count <=2)
            owner.characterAnim.PlayAction(owner.characterData.hold_1);
        else if(owner.ListItemHold.Count <=4)
            owner.characterAnim.PlayAction(owner.characterData.hold_2);
        else
            owner.characterAnim.PlayAction(owner.characterData.hold_3);
        UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractItem(true);
    }

    public override void Execute(Player owner)
    {
        base.Execute(owner);
        if (InputManager.Instance.IsInteract())
        {
            if (owner.stateMachine.CompareCurrentState(owner.stateMachine.idleState))
            {
                owner.characterAnim.PlayBase(owner.characterData.idle_1);
            }
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
            GameManager.Instance.RenUIPlayer.ChangeToIdle();
            GameManager.Instance.RenUIPlayer.ThrowItem();
            owner.ThrowItem();
            owner.SaveData();
            SaveGameManager.Instance.SaveData();
            return;
        }

        if (InputManager.Instance.IsStore())
        {
            if (owner.stateMachine.CompareCurrentState(owner.stateMachine.idleState))
            {
                owner.characterAnim.PlayBase(owner.characterData.idle_1);
            }
            owner.actionStateMachine.ChangeState(owner.actionStateMachine.noActionUpperState);
            GameManager.Instance.RenUIPlayer.ChangeToIdle();
            GameManager.Instance.RenUIPlayer.ThrowItem();
            owner.StoreItem();
            owner.SaveData();
            SaveGameManager.Instance.SaveData();
            return;
        }
    }
}
