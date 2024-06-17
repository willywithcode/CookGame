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
        this.SpawnItem(quantityTomato, PoolType.Tomato);
        this.SpawnItem(quantityBread, PoolType.Bread);
    }

    public void SpawnItem(int quantity, PoolType type)
    {
        for (int i = 0; i < quantity; i++)
        {
            Vector3 position = new Vector3(centerSpawner.x + UnityEngine.Random.Range(-width, width)
                , height
                , centerSpawner.y + UnityEngine.Random.Range(-length, length));
            GameUnit item = SimplePool.Spawn<GameUnit>(type);
            item.TF.position = position;
        }
    }
}
