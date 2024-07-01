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
    [SerializeField] private ClipTransition clipIdle;
    [SerializeField] private Transform leftHandTransform;
    [SerializeField] private Transform rightHandTransform;
    [SerializeField] private CharacterDataSO data;
    [SerializeField] private Vector3 leftHandPosition;
    [SerializeField] private Vector3 rightHandPosition;
    private GameObject currentItem;
    private DataItem<Item> curremtDataItem;
    private List<ItemStack> listHoldItem = new List<ItemStack>();
    private bool isEndSpawn = false;
    public bool IsEndSpawn => isEndSpawn;

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
            isEndSpawn = true;
            this.ChangeToIdle();
        };
    }
    public void SetCurrentItem(DataItem<Item> dataItem)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }
        currentItem = Instantiate(dataItem.prefabGameObject, leftHandTransform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localScale = Vector3.one * 0.3f;
        curremtDataItem = dataItem;
    }
    public void HoldItem(DataItem<Item> dataItem, int quantity)
    {
        GameObject item = Instantiate(dataItem.prefabGameObject, leftHandTransform);
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
        for (int i = 0; i < listHoldItem.Count; i++)
        {
            Destroy(listHoldItem[i].itemObject);
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
            HoldItem(SaveGameManager.Instance.dataItemContainer.dataItems[listItem[i].name],
                listItem[i].quantity);
            
        }
    }
}
