using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TrashBin : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private GameObject effect;
    [SerializeField] private Outline outline;
    public void ToggleOutLine(bool state) => outline.enabled = state;

    public void ThrowItemToTrashBin(List<ItemStack> items)
    {
        effect.SetActive(false);
        for (int i = 0; i < items.Count; i++)
        {
            ItemInWorld itemTF = items[i].itemObjectHolding;
            itemTF.TF.SetParent(this.point);
            itemTF.TF.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
                .OnComplete(() =>
                {
                    effect.SetActive(true);
                    itemTF.TF.eulerAngles = Vector3.zero;
                    itemTF.OnDespawn();
                });
        }
    }
} 