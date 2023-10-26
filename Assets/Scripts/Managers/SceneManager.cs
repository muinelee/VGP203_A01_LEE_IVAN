using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Singleton<SceneManager>
{
    private GameStateManager gsm;

    public void Initialize(GameStateManager gameStateManager)
    {
        this.gsm = gameStateManager;
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        gsm.SetState(GameState.Playing);
    }
}