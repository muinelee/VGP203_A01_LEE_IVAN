using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;

    [SerializeField] private PlayerController pc;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Pause.performed += ctx => OnPause(ctx);
        input.UI.Pause.canceled += ctx => OnPause(ctx);

        input.Keyboard.Accelerate.performed += ctx => OnAccelerate(ctx);
        input.Keyboard.Accelerate.canceled += ctx => OnAccelerate(ctx);
        input.Keyboard.Decelerate.performed += ctx => OnDecelerate(ctx);
        input.Keyboard.Decelerate.canceled += ctx => OnDecelerate(ctx);
        input.Keyboard.Steer.performed += ctx => OnSteer(ctx);
        input.Keyboard.Steer.canceled += ctx => OnSteer(ctx);
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Pause.performed -= ctx => OnPause(ctx);
        input.UI.Pause.canceled -= ctx => OnPause(ctx);

        input.Keyboard.Accelerate.performed -= ctx => OnAccelerate(ctx);
        input.Keyboard.Accelerate.canceled -= ctx => OnAccelerate(ctx);
        input.Keyboard.Decelerate.performed -= ctx => OnDecelerate(ctx);
        input.Keyboard.Decelerate.canceled -= ctx => OnDecelerate(ctx);
        input.Keyboard.Steer.performed -= ctx => OnSteer(ctx);
        input.Keyboard.Steer.canceled -= ctx => OnSteer(ctx);
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            GameManager.Instance.TogglePause();
        }
    }

    private void OnSteer(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            pc.steerInput = ctx.ReadValue<float>();
        }
        else if (ctx.canceled)
        {
            pc.steerInput = 0;
        }
        else
        {
            pc.steerInput = 0;
        }
    }

    private void OnAccelerate(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }

    private void OnDecelerate(InputAction.CallbackContext ctx)
    {
        throw new NotImplementedException();
    }
}