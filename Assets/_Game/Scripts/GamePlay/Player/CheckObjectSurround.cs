using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.Serialization.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class CheckObjectSurround : ACacheMonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    private List<Stove> stoves = new List<Stove>();
    private List<Tillage> tillages = new List<Tillage>();
    private bool hasCurrentStove = false;
    private bool hasCurrentTillage = false;
    private Stove currentStove;
    private Tillage currentTillage;
    private TeleDoor currentTeleDoor;
    private KnifeTable currentKnifeTable;
    private TrashBin currentTrashBin;

    private void Start()
    {
        UIGamePlay ui = UIManager.Instance.GetUI<UIGamePlay>();
        ui.AddCookButtonEvent(() =>
        {
            if(currentStove == null) return;
            if(player.ListItemHold.Count > 0)
            {
                currentStove.Cook(player.ListItemHold);
                GameManager.Instance.RenUIPlayer.ThrowItem();
                player.ListItemHold.Clear();
                player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
                player.SaveData();
                SaveGameManager.Instance.SaveData();
            }
            else
            {
                UIManager.Instance.GetUI<UIGamePlay>().PopUpText("You don't hold any item");
            }
            this.GetCurrentStove();
        });
        ui.AddFarmInteractEvent(() =>
        {
            if (currentTillage == null)
            {
                return;
            }
            if(!player.actionStateMachine.CompareCurrentState(player.actionStateMachine.noActionUpperState))
            {
                ui.PopUpText("You can't farm while holding item");
                return;
            }
            SelectedItem selectedItem = this.GetCurrentItemFromPlayer();
            if (!currentTillage.IsHavePlant)
            {
                if (selectedItem.HaveItem && selectedItem.DataItem.type == ItemType.Seed)
                {
                    currentTillage.Interact(SaveGameManager.GetPlant(selectedItem.DataItem.name));
                    selectedItem.RemoveItem();
                    return;
                }
                UIManager.Instance.GetUI<UIGamePlay>().PopUpText("You don't got seed");
            }
            else
            {
                currentTillage.Interact();
            }
        });
        ui.AddTeleportEvent(() =>
        {
            if(currentTeleDoor == null) return;
            UIManager.Instance.OpenUI<UIBlock>();
            TeleDoor door = currentTeleDoor;
            currentTeleDoor = null;
            player.character.enabled = false;
            DOVirtual.DelayedCall(0.3f, () => door.Teleport(player.TF));
        });
        ui.AddCuttingEvent(() =>
        {
            if(player.ListItemHold.Count == 0)
            {
                ui.PopUpText("You don't hold any item");
                return;
            }
            if(player.ListItemHold.Count > 1)
            {
                ui.PopUpText("You can only cut one item at a time");
                return;
            }
            if(player.ListItemHold[0].itemObjectHolding.ItemType != ItemType.Ingredient)
            {
                ui.PopUpText("You can only cut ingredient");
                return;
            }
            currentKnifeTable.Cut(player.ListItemHold[0]);
            GameManager.Instance.RenUIPlayer.ThrowItem();
            player.ListItemHold.Clear();
            player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
            player.SaveData();
            SaveGameManager.Instance.SaveData();
        });
        ui.AddTrashBinEvent(() =>
        {
            if(player.ListItemHold.Count == 0)
            {
                ui.PopUpText("You don't hold any item");
                return;
            }
            currentTrashBin.ThrowItemToTrashBin(player.ListItemHold);
            player.ListItemHold.Clear();
            GameManager.Instance.RenUIPlayer.ThrowItem();
            player.actionStateMachine.ChangeState(player.actionStateMachine.noActionUpperState);
            player.SaveData();
            SaveGameManager.Instance.SaveData();
        });
        ui.AddFishingEvent(() =>
        {
            if(player.ListItemHold.Count > 0)
            {
                ui.PopUpText("You can't fish while holding item");
                return;
            }
            player.stateMachine.ChangeState(player.stateMachine.idleState);
            player.actionStateMachine.ChangeState(player.actionStateMachine.startFishingState);
            ui.CloseDirectly();
            UIManager.Instance.OpenUI<UIFishing>();
        });
    }

    private void Update()
    {
        this.TF.position = player.TF.position;
        if(!player.actionStateMachine.CompareCurrentStateIsOrTypeOf(player.actionStateMachine.farmState))
            this.GetCurrentTillage();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Cache.GetTriggerPickable(other, out var pickable))
        {
            ItemInWorld item = (ItemInWorld) pickable;
            UIManager.Instance.GetUI<UIGamePlay>().PickupItemScreen.AddItem(item, other);
        }
        if (Cache.GetColliderStove(other, out Stove stove))
        {
            stoves.Add(stove);
            AssignOnDoneCooking(stove);
            if(hasCurrentStove) return;
            this.GetCurrentStove();
        } 
        if(Cache.GetColliderTillage(other, out Tillage tillage))
        {
            tillages.Add(tillage);
            this.GetCurrentTillage();
        }
        if(Cache.GetColliderTeleDoor(other, out TeleDoor teleDoor))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonTeleport(true);
            currentTeleDoor = teleDoor;
        }
        if(Cache.GetColliderShop(other, out Shop shop))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonViewStore(true);
            shop.ToggleOutLine(true);
        }
        if(Cache.GetColliderOrderContainer(other, out OrderContainer orderContainer))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonOrder(true);
            orderContainer.ToggleOutLine(true);
        }

        if (Cache.GetColliderKnifeTable(other, out KnifeTable knifeTable))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonKnifeTable(true);
            currentKnifeTable = knifeTable;
            currentKnifeTable.ToggleOutLine(true);
        }
        if(Cache.GetColliderTrashBin(other, out TrashBin trashBin))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonTrashBin(true);
            currentTrashBin = trashBin;
            trashBin.ToggleOutLine(true);
        }
        if(Cache.GetColliderFishingSite(other, out FishingSite fishingSite))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonFishingSite(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(Cache.GetTriggerPickable(other, out var pickable))
        {
            UIManager.Instance.GetUI<UIGamePlay>().PickupItemScreen.RemoveItem(other.GetInstanceID());
        }
        if (Cache.GetColliderStove(other, out Stove stove))
        {
            stoves.Remove(stove);
            UIManager.Instance.GetUI<UIGamePlay>().ToggleCookButton(false);
            if(stoves.Count == 0 || !CheckHaveUsableStove()) hasCurrentStove = false;
            this.GetCurrentStove();
        } 
        if(Cache.GetColliderTillage(other, out Tillage tillage))
        {
            tillages.Remove(tillage);
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonInteractFarm(false);
            if(tillages.Count == 0 || !CheckHaveInteractableTillage()) hasCurrentTillage = false;
            this.GetCurrentTillage();
        }
        if(Cache.GetColliderTeleDoor(other, out TeleDoor teleDoor))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonTeleport(false);
            currentTeleDoor = null;
        }
        if(Cache.GetColliderShop(other, out Shop shop))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonViewStore(false);
            shop.ToggleOutLine(false);
        }
        if(Cache.GetColliderOrderContainer(other, out OrderContainer orderContainer))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonOrder(false);
            orderContainer.ToggleOutLine(false);
        }

        if (Cache.GetColliderKnifeTable(other, out KnifeTable knifeTable))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonKnifeTable(false);
            knifeTable.ToggleOutLine(false);
            currentKnifeTable = null;
        }
        if(Cache.GetColliderTrashBin(other, out TrashBin trashBin))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonTrashBin(false);
            currentTrashBin = null;
            trashBin.ToggleOutLine(false);
        }
        if(Cache.GetColliderFishingSite(other, out FishingSite fishingSite))
        {
            UIManager.Instance.GetUI<UIGamePlay>().ToggleButtonFishingSite(false);
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
    private void AssignOnDoneCooking(Stove stove)
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
        SetCurrentStove(GetUsableStove());
        if (currentStove == null)
        {
            hasCurrentStove = false;
            return;
        }
        hasCurrentStove = true;
        UIGamePlay ui = UIManager.Instance.GetUI<UIGamePlay>();
        ui.ToggleCookButton(true);
    }
    public bool CheckHaveUsableStove()
    {
        return GetUsableStove() != null;
    }

    public Tillage GetInteractableTillage()
    {
        List<Tillage> temp = new List<Tillage>();
        for (int i = 0; i < tillages.Count; i++)
        {
            if (!tillages[i].IsHavePlant)
            {
                temp.Add(tillages[i]);
            }
            if(tillages[i].typeProcess != TypeProcess.Growing)
            {
                temp.Add(tillages[i]);
            }
        }
        if(temp.Count == 0) return null;
        float minDis = Vector3.Distance(player.TF.position, temp[0].TF.position);
        int index = 0;
        for(int i = 0; i < temp.Count; i++)
        {
            float dis = Vector3.Distance(player.TF.position, temp[i].TF.position);
            if(dis < minDis)
            {
                minDis = dis;
                index = i;
            }
        }
        return temp[index];
    }
    public void GetCurrentTillage()
    {
        this.SetCurrentTillage(GetInteractableTillage());
        if (currentTillage == null)
        {
            hasCurrentTillage = false;
            return;
        }
        hasCurrentTillage = true;
        UIGamePlay ui = UIManager.Instance.GetUI<UIGamePlay>();
        if (!currentTillage.IsHavePlant)
        {
            ui.ChangeIconFarmInteract(FarmInteractType.Plant);
        }
        else
        {
            if(currentTillage.typeProcess == TypeProcess.Water)
            {
                ui.ChangeIconFarmInteract(FarmInteractType.Water);
            }
            else if(currentTillage.typeProcess == TypeProcess.Harvest)
            {
                ui.ChangeIconFarmInteract(FarmInteractType.Harvest);
            }
        }
        ui.ToggleButtonInteractFarm(true);
    }
    public bool CheckHaveInteractableTillage()
    {
        return GetInteractableTillage() != null;
    }

    public SelectedItem GetCurrentItemFromPlayer()
    {
        return UIManager.Instance.GetUI<UIGamePlay>().GetCurrentSelectedItem();
    }
    public void SetCurrentTillage(Tillage tillage)
    {
        if (currentTillage != null)
        {
            currentTillage.meshRenderer.material = defaultMaterial;
            currentTillage.ToggleOutLine(false);
        }
        
        currentTillage = tillage;
        if (currentTillage != null)
        {
            currentTillage.meshRenderer.material = highlightMaterial;
            currentTillage.ToggleOutLine(true);
        }
    }
    public void SetCurrentStove(Stove stove)
    {
        if (currentStove != null)
        {
            currentStove.ToggleOutLine(false);
        }
        currentStove = stove;
        if (currentStove != null)
        {
            currentStove.ToggleOutLine(true);
        }
    }
}
