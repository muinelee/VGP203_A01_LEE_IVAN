using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;

    [SerializeField] private PlayerController pc;

    private bool isPaused = false;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Pause.performed += ctx => OnPause(ctx);
        input.UI.Pause.canceled += ctx => OnPause(ctx);
        input.Mouse.Shoot.performed += ctx => OnShoot(ctx);
        input.Mouse.Shoot.canceled += ctx => OnShoot(ctx);
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Pause.performed -= ctx => OnPause(ctx);
        input.UI.Pause.canceled -= ctx => OnPause(ctx);
        input.Mouse.Shoot.performed -= ctx => OnShoot(ctx);
        input.Mouse.Shoot.canceled -= ctx => OnShoot(ctx);
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        if (GameManager.Instance.CurrentGameState == GameState.PAUSE || 
            GameManager.Instance.CurrentGameState == GameState.GAMEOVER ||
            GameManager.Instance.CurrentGameState == GameState.MAIN) 
                return;

        if (ctx.performed)
        {
            pc.StartCharging();
        }
        if (ctx.canceled)
        {
            pc.HandleShooting();
        }
    }
}