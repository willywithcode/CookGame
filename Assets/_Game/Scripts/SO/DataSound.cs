using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
[CreateAssetMenu(fileName = "DataSound", menuName = "ScriptableObject/DataSound" )]
public class DataSound : ScriptableObject
{
    [FoldoutGroup("Farm")] public AudioClip soundPlant;
    [FoldoutGroup("Farm")] public AudioClip soundHarvest;
    [FoldoutGroup("Farm")] public AudioClip soundWater;
    [FoldoutGroup("UI")] public AudioClip buttonClick;
    [FoldoutGroup("BG")] public AudioClip bgm;
    [FoldoutGroup("GamePlay")] public AudioClip soundFire;
}
