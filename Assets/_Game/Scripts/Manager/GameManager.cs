using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Gameplay,
    Inventory
}
public class GameManager : Singleton<GameManager>
{
    private static GameState currentGameState;
    private void Start()
    {
        UIManager.Instance.OpenUI<UIGamePlay>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameManager.CompareCurrentState(GameState.Gameplay))
            {
                UIManager.Instance.OpenUI<UIInventory>();
                GameManager.ChangeState(GameState.Inventory);
            }
            else if (GameManager.CompareCurrentState(GameState.Inventory))
            {
                GameManager.ChangeState(GameState.Gameplay);
                UIManager.Instance.GetUI<UIInventory>().Close(0);
            }
        }
    }

    public static void ChangeState(GameState newState)
    {
        currentGameState = newState;
    }
    public static GameState GetState()
    {
        return currentGameState;
    }
    public static bool CompareCurrentState(GameState state)
    {
        return currentGameState == state;
    }
}
