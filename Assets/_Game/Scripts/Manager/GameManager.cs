using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Gameplay
}
public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        UIManager.Instance.OpenUI<UIGamePlay>();
    }
}
