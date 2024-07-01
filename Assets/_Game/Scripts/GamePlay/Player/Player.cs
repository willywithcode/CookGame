using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using ECM2;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Player : ACacheMonoBehauviour
{
    public Character character;
    public CharacterDataSO characterData;
    public FSTPlayer stateMachine;
    public FSTActionPlayer actionStateMachine;
    public CharacterAnim characterAnim;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private Vector3 leftHandPosition;
    [SerializeField] private Vector3 rightHandPosition;
    private List<ItemStack> listItemHold = new List<ItemStack>();
    public List<ItemStack> ListItemHold => listItemHold;
    private float turnToSmoothTime = 0.1f;
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
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref turnToSmoothTime, 0.3f);
        Vector3 moveDir = (Quaternion.Euler(0, targetAngle, 0) * Vector3.forward).normalized;
        return new Tuple<Vector3, float>(moveDir, angle);
    }
    public Tuple<Vector3, float> GetCurrentDirectionLocal()
    {
        return this.GetDirectionLocal(InputManager.Instance.GetMoveDirection().normalized);
    }
    
    public bool AddItemToInventory( int quantity, Item item)
    {
        return UIManager.Instance.GetUI<UIInventory>().AddItemToInventory(item.TypeItem, quantity);
    }
    public void HoldItem(DataItem<Item> dataItem, int quantity)
    {
        GameObject item = Instantiate(dataItem.prefabGameObject, leftHandTransform);
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
    public void HoldLeftHand(GameObject item)
    {
        item.transform.SetParent(leftHandTransform);
        float randomFactor = Random.value;
        item.transform.localPosition = Vector3.Lerp(Vector3.zero, leftHandPosition, randomFactor);
        item.transform.localScale = Vector3.one * 0.3f;
    }
    public void HoldRightHand(GameObject item)
    {
        item.transform.SetParent(rightHandTransform);
        float randomFactor = Random.value;
        item.transform.localPosition = Vector3.Lerp(Vector3.zero, rightHandPosition, randomFactor) ;
        item.transform.localScale = Vector3.one * 0.3f;
    }

    public void ThrowItem()
    {
        for (int i = 0; i < listItemHold.Count; i++)
        {
            Item item = listItemHold[i].item.prefab.ItemFactory.GetObject(listItemHold[i].quantity);
            /*item.TF.position = new Vector3(TF.position.x + Random.Range(0f,2f)
                , TF.position.y + 1
                , TF.position.z + Random.Range(0f,2f));*/
            item.TF.position = listItemHold[i].itemObject.transform.position;
            Destroy(listItemHold[i].itemObject);
        }
        listItemHold.Clear();
    }

    public void StoreItem()
    {
        if(listItemHold.Count == 0) return;
        for(int i = 0; i < listItemHold.Count; i++)
        {
            this.AddItemToInventory(listItemHold[i].quantity, listItemHold[i].item.prefab);
            
            Destroy(listItemHold[i].itemObject);
        }
        listItemHold.Clear();
    }

    public void LoadData()
    {
        var listItem = SaveGameManager.Instance.ItemPlayerHold ;
        if(listItem.Count <= 0) return;
        for(int i = 0; i < listItem.Count; i++)
        {
            HoldItem(SaveGameManager.Instance.dataItemContainer.dataItems[listItem[i].name],
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
    public ItemStack(DataItem<Item> item, int quantity, GameObject itemObject)
    {
        this.item = item;
        this.quantity = quantity;
        this.itemObject = itemObject;
    }
    public DataItem<Item> item;
    public int quantity;
    public GameObject itemObject;
}
