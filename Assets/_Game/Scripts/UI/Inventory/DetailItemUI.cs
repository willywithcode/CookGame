using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DetailItemUI : MonoBehaviour
{
    [SerializeField] private Image contentItemImg;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private ButtonCustom throwBtn;
    [SerializeField] private ButtonCustom throwAllBtn;
    [SerializeField] private ButtonCustom splitBtn;
    public void SetDetail(DataItem dataItem)
    {
        contentItemImg.gameObject.SetActive(true);
        contentItemImg.sprite = dataItem.icon;
        itemNameText.text = dataItem.name;
        descriptionText.text = dataItem.description;
    }

    public void ResetDetail()
    {
        contentItemImg.gameObject.SetActive(false);
        contentItemImg.sprite = null;
        itemNameText.text = "";
        descriptionText.text = "";
    }

    public void AddEventOnclickThrowBtn(UnityAction action)
    {
        this.throwBtn.customButtonOnClick += action;
    }

    public void AddEventOnclickThrowAllBtn(UnityAction action)
    {
        this.throwAllBtn.customButtonOnClick += action;
    }

    public void AddEventOnclickSplitBtn(UnityAction action)
    {
        this.splitBtn.customButtonOnClick += action;
    }
}
