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
    // RenUIPlayer and player
    [SerializeField] private RenUIPlayer renUIPlayer;
    [SerializeField] private Player player;
    public RenUIPlayer RenUIPlayer => renUIPlayer;
    public Player Player => player;
    // Current game state
    private static GameState currentGameState;
    // Start game
    private void Start()
    {
        UIManager.Instance.OpenUI<UIInventory>();
        UIManager.Instance.GetUI<UIInventory>().CloseDirectly();
        UIManager.Instance.GetUI<UIGamePlay>().CloseDirectly();
        UIManager.Instance.OpenUI<UILoading>();
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        #if UNITY_EDITOR
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
                UIManager.Instance.OpenUI<UIGamePlay>();
                UIManager.Instance.GetUI<UIInventory>().Close(0);
            }
        }
        #endif
    }
    // Change game state
    public static void ChangeState(GameState newState)
    {
        currentGameState = newState;
    }
    // Get current game state
    public static GameState GetState()
    {
        return currentGameState;
    }
    // Compare current game state
    public static bool CompareCurrentState(GameState state)
    {
        return currentGameState == state;
    }
    #if UNITY_EDITOR
    [Button]
    // Add item in inventory in editor
    public void AddItemInInventory([ValueDropdown(nameof(ValueDropdown))] string name, int quantity)
    {
        UIManager.Instance.GetUI<UIInventory>().AddItemToInventory(name, quantity);
    }
    public IEnumerable ValueDropdown()
    {
        return SaveGameManager.Instance.dataItemContainer.dataItems.Select(e => e.Key);
    }
    #endif
}
