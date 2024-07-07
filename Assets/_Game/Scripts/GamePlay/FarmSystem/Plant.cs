using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public enum TypeProcess
{
    Plant,
    Water, 
    Growing,
    Harvest
}
[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantData", order = 2)]
public class Plant : ScriptableObject
{
    public string namePlant;
    public float timeToGrow;
    public ObjectStagePlant[] plantGrowStages;
    public ObjectStagePlant seed;
    public int numberHarvest = 5;

    public async UniTask Growing(Tillage tillage)
    {
        float timeEachStage = timeToGrow / plantGrowStages.Length;
        for (int i = 0 ; i < plantGrowStages.Length; i++)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeEachStage));
            tillage.currentPlant.OnDespawn();
            tillage.currentPlant = plantGrowStages[i].PoolObjectPlant.GetObject();
            tillage.currentPlant.TF.SetParent(tillage.PointContainer);
            tillage.currentPlant.TF.localPosition = Vector3.zero;
            tillage.currentPlant.TF.localScale = Vector3.one;
            
        }
        tillage.typeProcess = TypeProcess.Harvest;
        GameManager.Instance.Player.checkObjectSurround.GetCurrentTillage();
    }
}
