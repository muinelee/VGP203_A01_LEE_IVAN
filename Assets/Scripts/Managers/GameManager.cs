using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public PlayerInputActions input;

    // References to other managers
    public GameStateManager GSM { get; private set; }
    public AudioManager AM { get; private set; }
    public InputManager IM { get; private set; }
    public SceneManager SM { get; private set; }
    public MenuManager MM { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputActions();

        // Initialize references to other managers
        if (!GSM) GSM = GameStateManager.Instance;
        if (!AM) AM = AudioManager.Instance;
        if (!IM) IM = InputManager.Instance;
        if (!SM) SM = SceneManager.Instance;
        if (!MM) MM = MenuManager.Instance;
        
        // Initialize other managers
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        GSM.Initialize();
        AM.Initialize(GSM, MM);
        IM.Initialize(GSM);
        SM.Initialize(GSM);
        MM.Initialize(AM, GSM, IM, SM);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
