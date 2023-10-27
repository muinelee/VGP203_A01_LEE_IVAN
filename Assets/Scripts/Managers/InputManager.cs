using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public GameStateManager gsm;
    public GameState CurrentState { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        var input = GameManager.Instance.input;
        input.UI.Pause.performed += ctx => OnPause(ctx);
        input.UI.Pause.canceled += ctx => OnPause(ctx);
    }

    private void OnDisable()
    {
        var input = GameManager.Instance.input;
        input.UI.Pause.performed -= ctx => OnPause(ctx);
        input.UI.Pause.canceled -= ctx => OnPause(ctx);
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && gsm.CurrentState == GameState.Playing)
        {
            gsm.SetState(GameState.Paused);
        }
        else if (ctx.performed && gsm.CurrentState == GameState.Paused)
        {
            gsm.SetState(GameState.Playing);
        }
    }
}