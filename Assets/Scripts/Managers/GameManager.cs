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
    [SerializeField] private PlayerController pc;
    [SerializeField] private MenuManager mm;
    [SerializeField] private EnemyManager em;

    public event Action OnGameStateChanged;
    private GameState _currentGameState;

    public GameState CurrentGameState
    {
        get => _currentGameState;
        set
        {
            if (_currentGameState != value)
            {
                _currentGameState = value;
                OnGameStateChanged?.Invoke();
            }
        }
    }

    #region Unity Methods
    protected override void Awake()
    {
        base.Awake();
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
            if (pc != null)
            {
                pc.OnProjectilesDepleted -= CheckGameOver;
            }

            if (em != null)
            {
                em.OnAllEnemiesDefeated -= CheckGameOver;
            }

            ChangeGameState(GameState.MAIN);
            Time.timeScale = 1;
        }
        else if (scene.name == "PlayScene")
        {
            pc = FindObjectOfType<PlayerController>();
            mm = FindObjectOfType<MenuManager>();
            em = FindObjectOfType<EnemyManager>();

            pc.OnProjectilesDepleted += CheckGameOver;
            em.OnAllEnemiesDefeated += CheckGameOver;

            ChangeGameState(GameState.PLAY);
            Time.timeScale = 1;
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
            ChangeGameState(GameState.PLAY);
        }
        else if (_currentGameState == GameState.PLAY)
        {
            Time.timeScale = 0;
            ChangeGameState(GameState.PAUSE);
        }
    }

    private void CheckGameOver()
    {
        Debug.Log("CheckGameOverCalled");
        Debug.Log("Remaining Projectiles: " + pc.RemainingProjectiles);
        Debug.Log("Remaining Enemies: " + em.RemainingEnemies);

        if (pc.RemainingProjectiles <=0 || em.RemainingEnemies <= 0)
        {
            Debug.Log("GameOver condition met.");
            GameOver();
            return;
        }
    }

    private void GameOver()
    {
        ChangeGameState(GameState.GAMEOVER);
        Debug.Log("Current Game State: " + CurrentGameState);
        Time.timeScale = 0;
        OnGameStateChanged?.Invoke();
    }
}