using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class RenUIPlayer : MonoBehaviour
{
    [SerializeField] private CharacterAnim characterAnim;
    [SerializeField] private ClipTransition clipIdle;
    [SerializeField] private Transform handTransform;
    private GameObject currentItem;
    private DataItem curremtDataItem;

    private void Start()
    {
        characterAnim.PlayBase(clipIdle, false);
    }
    public void SetCurrentItem(DataItem dataItem)
    {
        if (currentItem != null)
        {
            Destroy(currentItem);
        }
        currentItem = Instantiate(dataItem.prefabGameObject, handTransform);
        currentItem.transform.localPosition = Vector3.zero;
        currentItem.transform.localScale = Vector3.one * 0.3f;
        curremtDataItem = dataItem;
    }

    public void ResetItem()
    {
        Destroy(currentItem);
        currentItem = null;
        curremtDataItem = null;
    }
}
