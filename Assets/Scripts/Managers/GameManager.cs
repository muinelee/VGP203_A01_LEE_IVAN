using System;
using UnityEngine;

/*[DefaultExecutionOrder(-1)]*/
public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public PlayerInputActions input;

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputActions();
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
