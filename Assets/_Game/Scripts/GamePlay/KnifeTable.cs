using System.Collections;
using System.Collections.Generic;
using Animancer;
using DG.Tweening;
using UnityEngine;

public class KnifeTable : ACacheMonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private Transform pointContainer;
    [SerializeField] private AnimationClip clip;
    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimancerComponent anim;
    [SerializeField] private GameObject effect;
    private bool isCutting = false;
    public bool IsCutting => isCutting;
    public void ToggleOutLine(bool state) => outline.enabled = state;
    public void Interact()
    {
        anim.Play(clip);
    }
    public void Stop ()
    {
        anim.Play(idleClip);
    }

    public void Cut(ItemStack item)
    {
        if(isCutting) return;
        effect.SetActive(false);
        isCutting = true;
        ItemInWorld itemTF = item.itemObjectHolding;
        var board = SaveGameManager.Instance.dataItemContainer.cuttingBoards;
        if (board.ContainsKey(item.itemObjectHolding.ItemName))
        {
            string result = board[item.itemObjectHolding.ItemName];
            DataItem dataItem = SaveGameManager.GetDataItem(result);
            itemTF.TF.SetParent(this.pointContainer);
            itemTF.TF.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
                .OnComplete(() =>
                {
                    itemTF.TF.eulerAngles = Vector3.zero;
                    Interact();
                    DOVirtual.DelayedCall(2, () =>
                    {
                        itemTF.OnDespawn();
                        var obj = dataItem.prefab.ItemFactory.GetObject(parent: null, scale: Vector3.one);
                        obj.TF.SetParent(this.pointContainer);
                        obj.TF.localPosition = Vector3.zero;
                        obj.TF.localEulerAngles = Vector3.zero;
                        isCutting = false;
                        effect.SetActive(true);
                        Stop();
                    });
                });
        }
        else
        {
            itemTF.TF.SetParent(this.pointContainer);
            itemTF.TF.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
                .OnComplete(() =>
                {
                    itemTF.TF.eulerAngles = Vector3.zero;
                    Interact();
                    DOVirtual.DelayedCall(2, () =>
                    {
                        effect.SetActive(true);
                        itemTF.SetUp(true);
                        isCutting = false;
                        Stop();
                    });
                });
            
        }
    }
}
