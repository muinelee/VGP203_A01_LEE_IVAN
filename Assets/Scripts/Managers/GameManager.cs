using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MAIN,
    PLAY,
    PAUSE,
    GAMEOVER,
}

public class GameManager : Singleton<GameManager>
{
    public event Action<GameState> OnGameStateChanged;
    private GameState _currentGameState;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState != value)
            {
                _currentGameState = value;
                OnGameStateChanged?.Invoke(value);
            }
        }
    }

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    #endregion

    public void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;
        Debug.Log("Current Game State: " + CurrentGameState);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScene")
        {
            ChangeGameState(GameState.MAIN);
        }
        else if (scene.name == "PlayScene")
        {
            ChangeGameState(GameState.PLAY);
        }
    }

    public void LoadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void TogglePause()
    {
        if (SceneManager.GetActiveScene().name != "PlayScene") return;

        if (_currentGameState == GameState.PAUSE)
        {
            Time.timeScale = 1;
            CurrentGameState = GameState.PLAY;
        }
        else if (_currentGameState == GameState.PLAY)
        {
            Time.timeScale = 0;
            CurrentGameState = GameState.PAUSE;
        }
    }
}
