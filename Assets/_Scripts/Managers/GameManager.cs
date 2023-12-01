using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    MAIN,
    PLAY,
    PAUSE,
    GAMEOVER,
    WIN,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private SphereController sc;
    [SerializeField] private MenuManager mm;

    [SerializeField] public int currentLap = 0;
    [SerializeField] public int totalLaps = 3;
    [SerializeField] public float bestLapTime = float.MaxValue;

    public float countdownTime = 3f;
    public bool isCountingDown = false;
    public float lapTime = 0f;
    public bool isLapTimerRunning = false;

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
    }

    private void Update()
    {
        UpdateLapTimer();
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
            Time.timeScale = 1;
        }
        else if (scene.name == "PlayScene")
        {
            sc = FindObjectOfType<SphereController>();
            mm = FindObjectOfType<MenuManager>();

            ChangeGameState(GameState.PLAY);
            Time.timeScale = 1;

            UpdateLapCounter();
            ResetTimers();
            mm.countdownDisplay.SetActive(true);
            StartCoroutine(StartCountdown());
        }
    }

    private void ResetTimers()
    {
        countdownTime = 3f;
        lapTime = 0f;
        isLapTimerRunning = false;
    }

    public void RestartGame()
    {
        ResetTimers();
        LoadPlayScene();
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
            
            if (isCountingDown)
            {
                mm.countdownDisplay.SetActive(true);
                StartCoroutine(StartCountdown());
            }
            else
            {
                isLapTimerRunning = true;
            }
        }
        else if (_currentGameState == GameState.PLAY)
        {
            Time.timeScale = 0;
            ChangeGameState(GameState.PAUSE);
            isLapTimerRunning = false;
            if (isCountingDown)
            {
                StopCoroutine(StartCountdown());
                mm.countdownDisplay.SetActive(false);
            }
        }
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GAMEOVER);
        Time.timeScale = 0;
        OnGameStateChanged?.Invoke();
    }

    public void WinGame()
    {
        ChangeGameState(GameState.WIN);
        Time.timeScale = 0;
        OnGameStateChanged?.Invoke();
        mm.wm_bestLapTime.text = "BEST LAP TIME: " + FormatTime(TimeSpan.FromSeconds(bestLapTime));
    }

    private IEnumerator StartCountdown()
    {
        if (!isCountingDown)
        {
            FindObjectOfType<InputManager>().ToggleInput(false);
            yield return new WaitForSeconds(1f);
            isCountingDown = true;
        }

        while (countdownTime > 0)
        {
            mm.countdownText.text = countdownTime.ToString("0");
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        mm.countdownText.text = "GO!";
        FindObjectOfType<InputManager>().ToggleInput(true);
        StartLapTimer();
        yield return new WaitForSeconds(0.5f);
        mm.countdownDisplay.SetActive(false);
        isCountingDown = false;
        countdownTime = 3f; // Reset countdown for the next game start
    }

    private void StartLapTimer()
    {
        lapTime = 0f;
        isLapTimerRunning = true;
    }

    private void UpdateLapTimer()
    {
        if (!isLapTimerRunning) return;

        lapTime += Time.deltaTime;
        TimeSpan timeSpan = TimeSpan.FromSeconds(lapTime);
        mm.lapTimer.text = "LAP TIME: " + FormatTime(timeSpan);
    }

    public void LapCompleted()
    {
        if (lapTime < bestLapTime)
        {
            string lapTimeText = FormatTime(TimeSpan.FromSeconds(lapTime));
            mm.bestLapTime.text = "BEST LAP TIME: " + lapTimeText;
        }
        lapTime = 0f;
        currentLap++;
        UpdateLapCounter();

        if (currentLap >= totalLaps)
        {
            WinGame();
        }
    }

    public void UpdateLapCounter()
    {
        mm.currentLapCounter.text = "LAP: " + currentLap + "/" + totalLaps;
    }

    private string FormatTime(TimeSpan timeSpan)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D3}",
                             timeSpan.Minutes,
                             timeSpan.Seconds,
                             timeSpan.Milliseconds);
    }
}