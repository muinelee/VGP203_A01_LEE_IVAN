using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.TextCore.Text;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;

    [Header("Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeGame;
    [SerializeField] private Button quitButton;
    //[SerializeField] private Button saveButton;
    //[SerializeField] private Button loadButton;

    [Header("Sliders")]
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider sfxVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Dictionary<string, Slider> volumeSliders = new Dictionary<string, Slider>();

    [Header("Texts")]
    [SerializeField] private TMP_Text masterVolSliderText;
    [SerializeField] private TMP_Text sfxVolSliderText;
    [SerializeField] private TMP_Text musicVolSliderText;
    [SerializeField] private Dictionary<string, TMP_Text> volumeTexts = new Dictionary<string, TMP_Text>();

    [Header("Volume Defaults")]
    private const float defaultVolume = 0.75f;
    private const float volumeStep = 0.1f;

    private GameStateManager gsm;
    private InputManager im;
    private SceneManager sm;
    private AudioManager am;

    public void Initialize(AudioManager audioManager, GameStateManager gameStateManager, InputManager inputManager, SceneManager sceneManager)
    {
        this.am = audioManager;
        this.gsm = gameStateManager;
        this.im = inputManager;
        this.sm = sceneManager;

        mainMenu.SetActive(true);
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameOverMenu.SetActive(false);

        gsm.OnStateChanged += HandleGameStateChange;

        foreach (var key in volumeSliders.Keys)
        {
            float savedValue = PlayerPrefs.GetFloat(key, defaultVolume);
            volumeSliders[key].value = savedValue;
            volumeSliders[key].onValueChanged.AddListener((value) => OnVolumeChanged(key, value));
            UpdateVolumeUI(key, savedValue);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMixerVolume(key, savedValue);
            }
        }
    }

    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Playing:
                // Hide all menus or do something else
                break;
            case GameState.Paused:
                // Show the pause menu
                break;
            case GameState.GameOver:
                // Show the game-over menu
                break;
            default:
                break;
        }
    }

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        if(!gsm) gsm = GetComponent<GameStateManager>();
        if(!im) im = GetComponent<InputManager>();
        if(!sm) sm = GetComponent<SceneManager>();

        InitializeVolumeSliders();
        InitializeTexts();
        InitializeButtons();
    }
    
    private void InitializeVolumeSliders()
    {
        volumeSliders.Add("Master", masterVolSlider);
        volumeSliders.Add("SFX", sfxVolSlider);
        volumeSliders.Add("Music", musicVolSlider);        
    }

    private void InitializeTexts()
    {
        volumeTexts.Add("Master", masterVolSliderText);
        volumeTexts.Add("SFX", sfxVolSliderText);
        volumeTexts.Add("Music", musicVolSliderText);
    }

        private void InitializeButtons()
    {
        // Button Settings
        if (playButton)
        {
            playButton.onClick.AddListener(() => sm.LoadScene("PlayScene"));
        }
        if (settingsButton)
        {
            settingsButton.onClick.AddListener(() => ToggleMenu(mainMenu, settingsMenu));
        }
        if (mainMenuButton)
        {
            mainMenuButton.onClick.AddListener(() => sm.LoadScene("TitleScene"));
        }
        if (quitButton)
        {
            quitButton.onClick.AddListener(Quit);
        }
    }

    public void OnVolumeChanged(string type, float value)
    {
        if (volumeTexts.ContainsKey(type))
        {
            UpdateVolumeUI(type, value);
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMixerVolume(type, value);
        }
    }

    public void UpdateVolumeUI(string type, float value)
    {
        if (volumeTexts.ContainsKey(type))
        {
            value = Mathf.Round(value / volumeStep) * volumeStep;
            volumeTexts[type].text = (value * 100).ToString() + "%";
        }
    }

    private void ToggleMenu(GameObject currentMenu, GameObject nextMenu)
    {
        currentMenu.SetActive(false);
        nextMenu.SetActive(true);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}