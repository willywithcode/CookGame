using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailItemUI : MonoBehaviour
{
    [SerializeField] private Image contentItemImg;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    public void SetDetail(DataItem dataItem)
    {
        contentItemImg.sprite = dataItem.icon;
        itemNameText.text = dataItem.name;
        descriptionText.text = dataItem.description;
    }

    public void ResetDetail()
    {
        contentItemImg.sprite = null;
        itemNameText.text = "";
        descriptionText.text = "";
    }
}
