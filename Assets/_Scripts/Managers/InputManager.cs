using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [HideInInspector] public PlayerInputActions input;

    [SerializeField] private SphereController controller;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
        input.UI.Pause.performed += ctx => OnPause(ctx);
        input.UI.Pause.canceled += ctx => OnPause(ctx);

        input.Keyboard.Move.performed += ctx => OnAccelerate(ctx);
        input.Keyboard.Move.canceled += ctx => OnAccelerate(ctx);
        input.Keyboard.Steering.performed += ctx => OnSteer(ctx);
        input.Keyboard.Steering.canceled += ctx => OnSteer(ctx);
    }

    private void OnDisable()
    {
        input.Disable();
        input.UI.Pause.performed -= ctx => OnPause(ctx);
        input.UI.Pause.canceled -= ctx => OnPause(ctx);

        input.Keyboard.Move.performed -= ctx => OnAccelerate(ctx);
        input.Keyboard.Move.canceled -= ctx => OnAccelerate(ctx);
        input.Keyboard.Steering.performed -= ctx => OnSteer(ctx);
        input.Keyboard.Steering.canceled -= ctx => OnSteer(ctx);
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
            controller.steerInput = ctx.ReadValue<float>();
        }
        else if (ctx.canceled)
        {
            controller.steerInput = 0;
        }
    }

    private void OnAccelerate(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            controller.gasInput = ctx.ReadValue<float>();
        }
        else if (ctx.canceled)
        {
            controller.gasInput = 0;
        }
    }
}