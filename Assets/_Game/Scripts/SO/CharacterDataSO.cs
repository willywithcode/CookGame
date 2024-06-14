using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class CharacterDataSO : ScriptableObject
{
    [Title("Animation of Player")]
    public ClipTransition idle;
    public ClipTransition walk;
    public ClipTransition run;
    public ClipTransition jump;
    public ClipTransition fall;
    public ClipTransition land;
    public ClipTransition jog;
    public ClipTransition stopRunning;
    
    [BoxGroup("Speed")] public float walkSpeed = 5;
    [BoxGroup("Speed")] public float runSpeed = 10;
    [BoxGroup("Speed")] public float jogSpeed = 7;
}
