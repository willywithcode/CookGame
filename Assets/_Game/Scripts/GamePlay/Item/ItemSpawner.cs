using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Title("Item Spawner")]
    [SerializeField] private Vector2 centerSpawner;
    [SerializeField] private float width;
    [SerializeField] private float length;
    [SerializeField] private float height;
    
    [Title("Quantity")]
    [SerializeField] private int quantityTomato;
    [SerializeField] private int quantityBread;

    private void Start()
    {
        this.SpawnItem(quantityTomato, Constant.TOMATO_STRING);
        this.SpawnItem(quantityBread, Constant.BREAD_STRING);
        this.SpawnItem(10, Constant.CABBAGE_STRING);
    }

    public void SpawnItem(int quantity, string typeItem)
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector3 position = new Vector3(centerSpawner.x + UnityEngine.Random.Range(-width, width)
                , height
                , centerSpawner.y + UnityEngine.Random.Range(-length, length));
            Item item = SaveGameManager.Instance.dataItemContainer.dataItems[typeItem].prefab.ItemFactory.GetObject(5);
            item.TF.position = position;
        }
    }
}
