using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityTxt;
    public void Setup(DataItem dataItem, int quantity)
    {
        icon.sprite = dataItem.icon;
        quantityTxt.text = quantity.ToString();
    }
}
