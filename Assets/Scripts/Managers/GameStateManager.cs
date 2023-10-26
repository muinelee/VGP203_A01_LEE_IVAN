using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum GameState
{
    Title,
    Playing,
    Paused,
    GameOver,
}

public class GameStateManager : Singleton<GameStateManager>
{
    public event Action<GameState> OnStateChanged;
    public GameState CurrentState { get; private set; }

    public void Initialize()
    {
        CurrentState = GameState.Title;
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
        OnStateChanged?.Invoke(newState);
    }
}
