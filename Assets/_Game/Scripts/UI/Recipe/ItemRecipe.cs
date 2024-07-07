using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRecipe : ACacheMonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantity;
    
    public void Setup(Sprite icon, int quantity)
    {
        this.icon.sprite = icon;
        this.quantity.text = quantity.ToString();
    }
}
