using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    private GameManager gm;
    [HideInInspector] public PlayerInputActions input;

    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        gm = GameManager.Instance;
        input.Enable();
        input.UI.Pause.performed += ctx => OnPause(ctx);
        input.UI.Pause.canceled += ctx => OnPause(ctx);
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Pause.performed -= ctx => OnPause(ctx);
        input.UI.Pause.canceled -= ctx => OnPause(ctx);
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            gm.TogglePause();
        }
    }
}