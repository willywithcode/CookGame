using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "dataItemContainer", menuName = "ScriptableObjects/dataItemContainer", order = 1)]
public class DataItemContainer : SerializedScriptableObject
{
    public Dictionary<string, DataItem> dataItems = new Dictionary<string, DataItem>(StringComparer.OrdinalIgnoreCase);
    public List<Recipe> recipes;
#if UNITY_EDITOR
    public string link;
    public string tabNme;
    [Button]
    public async void GetDataFromSheet()
    {
        string result =  await Helper.GetJson(link, tabNme);
        Debug.Log(result);
        JArray jArray = JArray.Parse(result);
        recipes = new List<Recipe>();
        for (int i = 0; i < jArray.Count; i++)
        {
            JObject jObject = JObject.Parse(jArray[i].ToString());
            Recipe recipe = new Recipe();
            recipe.nameDish = jObject["Result"]!.ToString();
            recipe.time = int.Parse(jObject["Time"]!.ToString());
            recipe.ingredients = jObject["Recipe"]!.ToString();
            if(!ProcessIngredients(recipe.ingredients, out recipe.ingredients)) continue;
            recipes.Add(recipe);
        }
        AssetDatabase.SaveAssets();
        this.SetDirty();
    }
    public bool ProcessIngredients(string input, out string result)
    {
        result = "";
        string[] splitResult = input.Split("+");
        Dictionary<string, int> dataItemString = new Dictionary<string, int>();
        
        foreach (var obj in splitResult)
        {
            string nameObject = "";
            char[] delimiters = new char[] { ' ', '*'};
            string[] ingredients = obj.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0;i < ingredients.Length ; i ++)
            {
                string ingredient = ingredients[i];
                string firstChar = ingredient.Substring(0, 1).ToUpper();
                string restChars = ingredient.Substring(1).ToLower();
                ingredient = firstChar + restChars;
                ingredient = ingredient.Trim();
                if (!dataItems.ContainsKey(ingredient))
                {
                    Debug.LogError($"Ingredient {ingredient} not found at {link} - {tabNme} - ID : {i}");
                }
                else
                {
                    nameObject = ingredient;
                    break;
                }
            }
            if(nameObject == "")
            {
                return false;
            }

            int time = 1;
            foreach (var ingredient in ingredients)
            {
                if(int.TryParse(ingredient, out time))
                {
                    break;
                }
            }
            if(time == 0) time = 1;
            dataItemString.Add(nameObject, time);
        }
        var sortedPair = dataItemString.Keys.OrderBy(key => key);
        foreach (var key in sortedPair)
        {
            result += key + "*" + dataItemString[key] + "/";
        }
        return true;
    }
    
#endif
}
[Serializable]
public class DataItem
{
    // public string id, dung de luu du lieu game va get du lieu tu sheet, dictionary
    public string name;
    // Hien thi tren UI
    public string title;
    public string description;
    public Sprite icon;
    public Item prefab;
    public ItemHolding prefabGameObject;
    public ItemType type;
    public bool isStackable;
    [ShowIf("@(isStackable)")]public int maxStack;
}
public enum ItemType
{
    None,
    Food,
    Ingredient
}
[Serializable] 
public class Recipe
{
    
    public string nameDish;
    public int time;
    public string ingredients;
    public bool CanGetItem(string result, out DataItem item)
    {
        item = null;
        if (SaveGameManager.Instance.dataItemContainer.dataItems.ContainsKey(result))
        {
            item = SaveGameManager.GetDataItem(result);
            return true;
        }

        return false;
    }

    public bool GetResult(Dictionary<string,int> dataItemString, out DataItem item, out int time)
    {
        item = null;
        time = 0;
        string[] splitResult = ingredients.Split("/");
        int numOfIngredients = splitResult.Length - 1;
        if(numOfIngredients != dataItemString.Count) return false;
        for (int i = 0 ; i < splitResult.Length; i++)
        {
            string[] split = splitResult[i].Split("*");
            if(split.Length < 2) continue;
            string nameObject = split[0];
            int quantity = int.Parse(split[1]);
            if(!dataItemString.ContainsKey(nameObject) || dataItemString[nameObject] < quantity)
            {
                return false;
            }
        }
        string firstChar = nameDish.Substring(0, 1).ToUpper();
        string restChars = nameDish.Substring(1).ToLower();
        string normalNameDish = firstChar + restChars;
        if (CanGetItem(normalNameDish, out item))
        {
            time = this.time;
            return true;
        }
        return false;

    }
}
