using System.Collections;
using System.Collections.Generic;
using Animancer;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData", order = 1)]
public class CharacterDataSO : ScriptableObject
{
    [Title("Animation of Player")]
    [FoldoutGroup("Idle")] public ClipTransition idle_1;
    [FoldoutGroup("Idle")] public ClipTransition idle_2;
    [FoldoutGroup("Idle")] public ClipTransition idle_3;
    [FoldoutGroup("Move")] public ClipTransition walk;
    [FoldoutGroup("Move")] public ClipTransition run;
    [FoldoutGroup("Jump")] public ClipTransition jump;
    [FoldoutGroup("Jump")] public ClipTransition jumpRolling;
    [FoldoutGroup("Fall")] public ClipTransition fallNormal;
    [FoldoutGroup("Fall")] public ClipTransition fallRolling;
    [FoldoutGroup("Land")] public ClipTransition landNormal;
    [FoldoutGroup("Land")] public ClipTransition landRolling;
    [FoldoutGroup("Move")] public ClipTransition jog;
    [FoldoutGroup("StopRun")] public ClipTransition stopRunning;
    [FoldoutGroup("StopRun")] public ClipTransition stopWalking;
    [FoldoutGroup("StopRun")] public ClipTransition stopJogging;
    [FoldoutGroup("Punch Attack")] public ClipTransition punchRightAttack;
    [FoldoutGroup("Punch Attack")] public ClipTransition punchLeftAttack;
    [FoldoutGroup("Kick Attack")] public ClipTransition kickAttack_1;
    [FoldoutGroup("Kick Attack")] public ClipTransition kickAttack_2;
    [FoldoutGroup("Kick Attack")] public ClipTransition kickAttack_3;
    [FoldoutGroup("Jump Attack")] public ClipTransition jumpAttack;
    [FoldoutGroup("Hold")] public ClipTransition hold_1;
    [FoldoutGroup("Hold")] public ClipTransition hold_2;
    [FoldoutGroup("Hold")] public ClipTransition hold_3;
    [FoldoutGroup("Spawn")] public ClipTransition spawn;
    [FoldoutGroup("Farm")] public ClipTransition plantSeed;
    [FoldoutGroup("Farm")] public ClipTransition water;
    [FoldoutGroup("Farm")] public ClipTransition harvest;
    [FoldoutGroup("Farm")] public ClipTransition startFishing;
    [FoldoutGroup("Farm")] public ClipTransition pullFish;
    [Title("Speed")]
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float jogSpeed = 7;
    public float stopRunSpeed = 1;
    public float heightEnoughForLanding = 4;
}
