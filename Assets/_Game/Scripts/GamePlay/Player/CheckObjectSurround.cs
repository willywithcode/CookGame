using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckObjectSurround : ACacheMonoBehaviour
{
    [SerializeField] private Player player;
    private List<Stove> stoves = new List<Stove>();
    private bool hasCurrentStove = false;
    private void Update()
    {
        this.TF.position = player.TF.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Cache.GetTriggerPickable(other, out var pickable))
        {
            Item item = (Item) pickable;
            Debug.Log("check");
            UIManager.Instance.GetUI<UIGamePlay>().PickupItemScreen.AddItem(SaveGameManager.GetDataItem(item.ItemName)
                , () =>
                {
                    pickable.PickUp((param) =>  AddToInventory(item.quantity, (Item) pickable));
                }, other, item.quantity);
            // UIManager.Instance.GetUI<UIGamePlay>().PickupItemScreen.Check();
        }
        if (other.TryGetComponent<Stove>(out Stove stove))
        {
            stoves.Add(stove);
            AssignOnDoneCooking(stove);
            if(hasCurrentStove) return;
            this.GetCurrentStove();
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if(Cache.GetTriggerPickable(other, out var pickable))
        {
            UIManager.Instance.GetUI<UIGamePlay>().PickupItemScreen.RemoveItem(other);
        }
        if (other.TryGetComponent<Stove>(out Stove stove))
        {
            stoves.Remove(stove);
            UIManager.Instance.GetUI<UIGamePlay>().ToggleCookButton(false);
            if(stoves.Count == 0 || !CheckHaveUsableStove()) hasCurrentStove = false;
            this.GetCurrentStove();
        } 
    }
    public Stove GetUsableStove()
    {
        for (int i = 0 ; i < stoves.Count; i++)
        {
            if(!stoves[i].IsCooking)
            {
                return stoves[i];
            }
        }
        return null;
    }
    public void AssignOnDoneCooking(Stove stove)
    {
        if(stove.OnDoneCooking != null) return;
        stove.OnDoneCooking += (s) =>
        {
            if (stoves.Contains(s))
            {
                this.GetCurrentStove();
            }
        };
    }
    public void GetCurrentStove()
    {
        Stove currentStove = GetUsableStove();
        if (currentStove == null)
        {
            hasCurrentStove = false;
            return;
        }
        hasCurrentStove = true;
        UIGamePlay ui = UIManager.Instance.GetUI<UIGamePlay>();
        ui.ToggleCookButton(true);
        ui.AddCookButtonEvent(() =>
        {
            if(player.ListItemHold.Count > 0)
            {
                currentStove.Cook(player.ListItemHold);
                GameManager.Instance.RenUIPlayer.ThrowItem();
                player.ListItemHold.Clear();
                player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
                player.SaveData();
                SaveGameManager.Instance.SaveData();
            }
            this.GetCurrentStove();
        });
    }
    public bool CheckHaveUsableStove()
    {
        return GetUsableStove() != null;
    }
    public bool AddToInventory(int quantity, Item item)
    {
        return GameManager.Instance.Player.AddItemToInventory(quantity, item);
    }
}
