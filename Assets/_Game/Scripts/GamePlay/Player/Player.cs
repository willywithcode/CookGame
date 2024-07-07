using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using ECM2;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : ACacheMonoBehaviour
{
    public Character character;
    public CharacterDataSO characterData;
    public FSTPlayer stateMachine;
    public FSTActionPlayer actionStateMachine;
    public CharacterAnim characterAnim;
    public CheckObjectSurround checkObjectSurround;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Vector3 leftHandPosition;
    [SerializeField] private Vector3 rightHandPosition;
    [SerializeField] private GameObject waterCan;
    [SerializeField] private GameObject fishingRod;
    [SerializeField] private float smoothTime;
    private List<ItemStack> listItemHold = new List<ItemStack>();
    public List<ItemStack> ListItemHold => listItemHold;
    private float turnToSmoothTime = 0.1f;
    private Tween tween;

    private void Start()
    {
        this.RegisterListener(EventID.OnPickUpItem, param =>
        {
            if( !(param is PickUpItemBtn) || param == null) return;
            PickUpItemBtn item = ((PickUpItemBtn)param);
            this.AddItemToInventory(item.Data.quantity, SaveGameManager.GetDataItem(item.Data.ItemName).prefab);
            item.Data.PickUp();
        });
        this.RegisterListener(EventID.OnHoldingItem, (param) =>
        {
            UIInventory inventory = UIManager.Instance.GetUI<UIInventory>();
            this.HoldItem(inventory.CurrentSelectedItem.DataItem, 1);
            this.actionStateMachine.ChangeState(actionStateMachine.holdState);
            inventory.ChangeTextNumHoldItem(listItemHold.Count);
            this.SaveData();
        });
    }

    public void ChangeSpeed(float speed)
    {
        character.maxWalkSpeed = speed;
    }
    public void Rotate()
    {
        Tuple<Vector3, float> directionLocal = this.GetCurrentDirectionLocal();
        playerTransform.rotation = Quaternion.Euler(0, directionLocal.Item2, 0);
    }
    public  Tuple<Vector3, float> GetDirectionLocal(Vector3 direction)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnToSmoothTime, smoothTime);
        Vector3 moveDir = (Quaternion.Euler(0, targetAngle, 0) * Vector3.forward).normalized;
        return new Tuple<Vector3, float>(moveDir, angle);
    }
    public Tuple<Vector3, float> GetCurrentDirectionLocal()
    {
        return this.GetDirectionLocal(InputManager.Instance.GetMoveDirection().normalized);
    }
    
    public bool AddItemToInventory( int quantity, Item item)
    {
        return UIManager.Instance.GetUI<UIInventory>().AddItemToInventory(item.ItemName, quantity);
    }
    public void Teleport()
    {
        tween?.Kill();
        character.enabled = false;
        tween = DOVirtual.DelayedCall(0.5f, () => character.enabled = true);
    }
    public void StartWaterPlant()
    {
        waterCan.SetActive(true);
    }
    public void DoneWaterPlant()
    {
        waterCan.SetActive(false);
    }
    public void SetActiveFishingRod(bool isActive)
    {
        fishingRod.SetActive(isActive);
    }
    public void HoldItem(DataItem dataItem, int quantity)
    {
        ItemInWorld item = (ItemInWorld)dataItem.prefab.ItemFactory.GetObject();
        item.SetUp(false);
        listItemHold.Add(new ItemStack(dataItem, quantity, item));
        int random = UnityEngine.Random.Range(0, 2);
        if (random == 0)
        {
            HoldLeftHand(item);
        }
        else
        {
            HoldRightHand(item);
        }
    }
    public void HoldLeftHand(ItemInWorld item)
    {
        item.TF.SetParent(leftHandTransform);
        float randomFactor = Random.value;
        item.TF.localPosition = Vector3.Lerp(Vector3.zero, leftHandPosition, randomFactor);
        item.TF.localScale = Vector3.one * 0.3f;
    }
    public void HoldRightHand(ItemInWorld item)
    {
        item.TF.SetParent(rightHandTransform);
        float randomFactor = Random.value;
        item.TF.localPosition = Vector3.Lerp(Vector3.zero, rightHandPosition, randomFactor) ;
        item.TF.localScale = Vector3.one * 0.3f;
    }

    public void ThrowItem()
    {
        for (int i = 0; i < listItemHold.Count; i++)
        {
            listItemHold[i].itemObjectHolding.SetUp(true);
            listItemHold[i].itemObjectHolding.ReturnRoot();
        }
        listItemHold.Clear();
    }

    public void StoreItem()
    {
        if(listItemHold.Count == 0) return;
        for(int i = 0; i < listItemHold.Count; i++)
        {
            this.AddItemToInventory(listItemHold[i].quantity, listItemHold[i].item.prefab);
            listItemHold[i].itemObjectHolding.OnDespawn();
        }
        listItemHold.Clear();
    }

    public void LoadData()
    {
        var listItem = SaveGameManager.Instance.ItemPlayerHold ;
        if(listItem.Count <= 0) return;
        for(int i = 0; i < listItem.Count; i++)
        {
            HoldItem(SaveGameManager.GetDataItem(listItem[i].name),
                listItem[i].quantity);
        }
    }

    public bool CheckHaveDataItem()
    {
        return listItemHold.Count > 0;
    }
    public void SaveData()
    {
        List<ItemData> listItem = new List<ItemData>();
        if (listItemHold.Count == 0)
        {
            SaveGameManager.Instance.ItemPlayerHold = listItem;
            return;
        }
        for (int i = 0; i < listItemHold.Count; i++)
        {
            listItem.Add(new ItemData(listItemHold[i].item.name, listItemHold[i].quantity));
        }
        SaveGameManager.Instance.ItemPlayerHold = listItem;
    }
}

public struct ItemStack
{
    public ItemStack(DataItem item, int quantity, ItemInWorld itemObjectHolding)
    {
        this.item = item;
        this.quantity = quantity;
        this.itemObjectHolding = itemObjectHolding;
    }
    public DataItem item;
    public int quantity;
    public ItemInWorld itemObjectHolding;
}
