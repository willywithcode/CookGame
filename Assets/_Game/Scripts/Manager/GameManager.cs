using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    Gameplay,
    Inventory
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] private RenUIPlayer renUIPlayer;
    [SerializeField] private Player player;
    public RenUIPlayer RenUIPlayer => renUIPlayer;
    public Player Player => player;
    private static GameState currentGameState;
    private void Start()
    {
        UIManager.Instance.OpenUI<UIGamePlay>();
        UIManager.Instance.OpenUI<UIInventory>();
        UIManager.Instance.GetUI<UIInventory>().Close(0);
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
    #if UNITY_EDITOR
    [Button]
    public void AddItemInInvenroty([ValueDropdown(nameof(ValueDropdown))] string name, int quantity)
    {
        UIManager.Instance.GetUI<UIInventory>().AddItemToInventory(name, quantity);
    }
    public IEnumerable ValueDropdown()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(e => e.Value.name);
    }
    #endif
}
