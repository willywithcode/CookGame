using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseFollower : MonoBehaviour
{
    
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI quantityText;
    private InventoryItem currentInventoryItem;
    public InventoryItem CurrentInventoryItem => currentInventoryItem;
    public void SetFollower(DataItem dataItem, int quantity, InventoryItem item)
    {
        this.currentInventoryItem = item;
        image.sprite = dataItem.icon;
        if (!dataItem.isStackable || quantity < 2)
        {
            quantityText.gameObject.SetActive(false);
            return;
        }
        quantityText.gameObject.SetActive(true);
        quantityText.text = quantity.ToString();
    }

    public void FollowMouse()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            Input.mousePosition,
            canvas.worldCamera,
            out position
        );
        transform.position = canvas.transform.TransformPoint(position);
    }
}
