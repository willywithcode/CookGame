using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class Stove : ACacheMonoBehaviour
{
    public UnityAction<Stove> OnDoneCooking;
    [SerializeField] private GameObject effectFire;
    [SerializeField] private Transform pointCook;
    [SerializeField] private GameObject effectDone;
    [SerializeField] private Outline outline;
    private List<ItemStack> items = new List<ItemStack>();
    private bool isCooking = false;
    private List<ItemInWorld> listItems = new List<ItemInWorld>();
    private DataItem result;
    public bool IsCooking => isCooking;
    public void ToggleOutLine(bool state) => outline.enabled = state;
    public void ToggleFire(bool value)
    {
        effectFire.SetActive(value);
    }

    public void Cook(List<ItemStack> items)
    {
        if(isCooking) return;
        listItems.Clear();
        isCooking = true;
        this.ToggleFire(true);
        this.effectDone.SetActive(false);
        this.items = items;
        for(int i = 0 ; i < items.Count; i++)
        { 
            ItemInWorld itemTF = items[i].itemObjectHolding;
            listItems.Add(itemTF);
           itemTF.TF.SetParent(this.pointCook);
           itemTF.TF.DOLocalJump(Vector3.zero, 1, 1, 0.5f)
               .OnComplete(() =>
           {
               itemTF.TF.eulerAngles = Vector3.zero;
           });
        }
        this.GetResultFGetFromRecipe();
    }
    public void StopCook()
    {
        isCooking = false;
        OnDoneCooking?.Invoke(this);
        this.ToggleFire(false);
        this.effectDone.SetActive(true);
        this.SpawnResult(result);
    }

    public void GetResultFGetFromRecipe()
    {
        List<Recipe> recipes = SaveGameManager.Instance.dataItemContainer.recipes;  
        var dataItemString = ProcessIngredient();
        for(int i = 0; i < recipes.Count; i++)
        {
           if(recipes[i].GetResult(dataItemString, out DataItem result, out int time))
           {
                this.result = result;
                DOVirtual.DelayedCall(time, () =>
                {
                     this.StopCook();
                });
                return;
           }
        }
        result = SaveGameManager.GetDataItem(Constant.BURNT_MEAT_STRING);
        DOVirtual.DelayedCall(20, () =>
        {
            this.StopCook();
        });
    }

    public void SpawnResult(DataItem item)
    {
        var obj = item.prefab.ItemFactory.GetObject(parent: null, scale: Vector3.one);
        obj.TF.SetParent(this.pointCook);
        foreach (var ingredient in listItems)
        {
            ingredient.OnDespawn();
        }
        // var newPos = new Vector3( Random.Range(0f,2f)
        //     , 1
        //     , Random.Range(0f,2f));
        obj.TF.localPosition = Vector3.zero;
        obj.TF.eulerAngles = Vector3.zero;
        // obj.TF.DOLocalJump(newPos, 1, 1, 0.5f);
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
