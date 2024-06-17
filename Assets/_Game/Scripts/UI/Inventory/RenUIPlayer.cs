using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;

public class RenUIPlayer : MonoBehaviour
{
    [SerializeField] private CharacterAnim characterAnim;
    [SerializeField] private ClipTransition clipIdle;

    private void Start()
    {
        characterAnim.PlayBase(clipIdle, false);
    }
}
