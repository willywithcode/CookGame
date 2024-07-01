using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Stove : ACacheMonoBehauviour
{
    public UnityAction<Stove> OnDoneCooking;
    [SerializeField] private GameObject effectFire;
    private List<ItemStack> items = new List<ItemStack>();
    private bool isCooking = false;
    private DataItem<Item> result;
    public bool IsCooking => isCooking;
    public void ToggleFire(bool value)
    {
        effectFire.SetActive(value);
    }

    public void Cook(List<ItemStack> items)
    {
        if(isCooking) return;
        isCooking = true;
        this.ToggleFire(true);
        this.items = items;
        for(int i = 0 ; i < items.Count; i++)
        { 
            Transform itemTF = items[i].itemObject.transform;
           itemTF.SetParent(this.TF);
           itemTF.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
               .OnComplete(() =>
           {
               Destroy(itemTF.gameObject);
           });
        }
        this.GetResultFGetFromRecipe();
    }
    public void StopCook()
    {
        isCooking = false;
        OnDoneCooking?.Invoke(this);
        this.ToggleFire(false);
        this.SpawnResult(result);
    }

    public void GetResultFGetFromRecipe()
    {
        List<Recipe> recipes = SaveGameManager.Instance.dataItemContainer.recipes;  
        var dataItemString = ProcessIngredient();
        for(int i = 0; i < recipes.Count; i++)
        {
           if(recipes[i].GetResult(dataItemString, out DataItem<Item> result, out int time))
           {
               Debug.Log("Result: " + result.name);
                this.result = result;
                DOVirtual.DelayedCall(time, () =>
                {
                     this.StopCook();
                });
                return;
           }
        }
        result = SaveGameManager.Instance.dataItemContainer.dataItems[Constant.BURNT_MEAT_STRING];
        DOVirtual.DelayedCall(20, () =>
        {
            this.StopCook();
        });
    }

    public void SpawnResult(DataItem<Item> item)
    {
        var obj = item.prefab.ItemFactory.GetObject();
        obj.TF.SetParent(this.TF);
        var newPos = new Vector3( Random.Range(0f,2f)
            , 1
            , Random.Range(0f,2f));
        obj.TF.localPosition = Vector3.zero;
        obj.TF.DOLocalJump(newPos, 1, 1, 0.5f);
    }
    public  Dictionary<string, int> ProcessIngredient()
    {
        Dictionary<string, int> dataItemString = new Dictionary<string, int>();
        for (int i = 0; i < items.Count; i++)
        {
            if(dataItemString.ContainsKey(items[i].item.name))
            {
                dataItemString[items[i].item.name] += items[i].quantity;
            }
            else
            {
                dataItemString.Add(items[i].item.name, items[i].quantity);
            }
        }

        return dataItemString;
    }
}
