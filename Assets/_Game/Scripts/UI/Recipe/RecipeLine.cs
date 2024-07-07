using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeLine : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private ItemRecipe itemRecipePrefab;
    [SerializeField] private GameObject cookIconPrefab;

    public void Setup(List<DataItem> listIngredients, List<int> listQuantity, DataItem resultItem)
    {
        for (int i = 0; i < listIngredients.Count; i++)
        {
            ItemRecipe itemRecipe = Instantiate(itemRecipePrefab, container);
            itemRecipe.Setup(listIngredients[i].icon, listQuantity[i]);
        }
        var cookIcon = Instantiate(cookIconPrefab, container);
        ItemRecipe itemResult = Instantiate(itemRecipePrefab, container);
        itemResult.Setup(resultItem.icon, 1);
    }
}
