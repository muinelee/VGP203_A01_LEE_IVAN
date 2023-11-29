using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public InputManager inputManager;
    public WheelController[] wheels;

    [Header("Car Specs")]
    public float wheelBase;
    public float rearTrack;
    public float turnRadius;

    [Header("Inputs")]
    public float steerInput;
    public float accelerateInput;

    public float ackermannAngleLeft;
    public float ackermannAngleRight;

    private void Update()
    {
        // Turning Right
        if (steerInput < 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
        }
        // Turning Left
        else if (steerInput > 0)
        {
            ackermannAngleLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius - (rearTrack / 2))) * steerInput;
            ackermannAngleRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (turnRadius + (rearTrack / 2))) * steerInput;
        }
        // Not Turning
        else
        {
            ackermannAngleLeft = 0;
            ackermannAngleRight = 0;
        }

        foreach (WheelController w in wheels)
        {
            if (w.wheelFrontLeft)
            {
                w.steerAngle = ackermannAngleLeft;
            }
            else if (w.wheelFrontRight)
            {
                w.steerAngle = ackermannAngleRight;
            }
        }

        // receive acceleration input
        foreach (WheelController w in wheels)
        {
            if (w.wheelFrontLeft || w.wheelFrontRight)
            {
                w.accelerateInput = accelerateInput;
            }
        }
    }
}