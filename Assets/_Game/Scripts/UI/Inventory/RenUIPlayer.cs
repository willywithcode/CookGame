using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RenUIPlayer : MonoBehaviour
{
    [SerializeField] private CharacterAnim characterAnim;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private CharacterDataSO data;
    [SerializeField] private Vector3 leftHandPosition;
    [SerializeField] private Vector3 rightHandPosition;
    private Item currentItem;
    private DataItem curremtDataItem;
    private List<ItemStack> listHoldItem = new List<ItemStack>();

    private void Start()
    {
        if (listHoldItem.Count > 0) DOVirtual.DelayedCall(1, Hold);
        else this.ChangeToIdle();
    }

    public void ChangeToIdle()
    {
        /*aracterAnim.FadeOutUpperBody();*/
        characterAnim.PlayBase(data.idle_1, false);
    }

    public void SpawnPlayer()
    {
        if(listHoldItem.Count > 0) return;
        characterAnim.PlayBase(data.spawn, false).Events.OnEnd = () =>
        {
            this.ChangeToIdle();
        };
    }
    public void SetCurrentItem(DataItem dataItem)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }
        currentItem = dataItem.prefabGameObject.ItemFactory.GetObject(scale:Vector3.one * 0.3f);
        currentItem.TF.localPosition = Vector3.zero;
        curremtDataItem = dataItem;
    }
    public void HoldItem(DataItem dataItem, int quantity)
    {
        ItemHolding item = (ItemHolding)dataItem.prefabGameObject.ItemFactory.GetObject();
        listHoldItem.Add(new ItemStack(dataItem, quantity, item));
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
    public void HoldLeftHand(ItemHolding item)
    {
        item.TF.SetParent(leftHandTransform);
        float randomFactor = Random.value;
        item.TF.localPosition = Vector3.Lerp(Vector3.zero, leftHandPosition, randomFactor);
        item.TF.localScale = Vector3.one * 0.3f;
    }
    public void HoldRightHand(ItemHolding item)
    {
        item.TF.SetParent(rightHandTransform);
        float randomFactor = Random.value;
        item.TF.localPosition = Vector3.Lerp(Vector3.zero, rightHandPosition, randomFactor) ;
        item.TF.localScale = Vector3.one * 0.3f;
    }
    public void ThrowItem()
    {
        for (int i = 0; i < listHoldItem.Count; i++)
        {
            listHoldItem[i].itemObjectHolding.OnDespawn();
        }
        listHoldItem.Clear();
        this.ChangeToIdle();
    }
    public void Hold()
    {
        if(listHoldItem.Count <= 2) 
            characterAnim.PlayBase(data.hold_1);
        else if(listHoldItem.Count <= 4) 
            characterAnim.PlayBase(data.hold_2);
        else 
            characterAnim.PlayBase(data.hold_3);
    }

    public void ResetItem()
    {
        Destroy(currentItem);
        currentItem = null;
        curremtDataItem = null;
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
}
