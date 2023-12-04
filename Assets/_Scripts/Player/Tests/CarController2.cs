using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController2 : MonoBehaviour
{
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private float currentSteeringAngle;
    private float currentBreakForce;
    private bool isBreaking;

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;
    [SerializeField] private float downforceMultiplier = 100f; // Adjustable downforce multiplier

    [SerializeField] private WheelCollider FLWheelCollider;
    [SerializeField] private WheelCollider FRWheelCollider;
    [SerializeField] private WheelCollider BLWheelCollider;
    [SerializeField] private WheelCollider BRWheelCollider;

    [SerializeField] private Transform FLWheelTransform;
    [SerializeField] private Transform FRWheelTransform;
    [SerializeField] private Transform BLWheelTransform;
    [SerializeField] private Transform BRWheelTransform;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        ApplyDownforce();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL); // Get the input value directly
        verticalInput = Input.GetAxis(VERTICAL); // Get the input value directly
        isBreaking = Input.GetKey(KeyCode.Space); // Use GetKey instead of GetKeyDown for continuous brake checking
    }

    private void HandleMotor()
    {
        // Only apply torque if there is vertical input
    if (Mathf.Abs(verticalInput) > Mathf.Epsilon) // Mathf.Epsilon is a very small number, effectively zero
        {
            float torque = verticalInput * motorForce;
            FLWheelCollider.motorTorque = torque;
            FRWheelCollider.motorTorque = torque;
        }
        else // No input, no torque
        {
            FLWheelCollider.motorTorque = 0;
            FRWheelCollider.motorTorque = 0;
        }

        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        FLWheelCollider.brakeTorque = currentBreakForce;
        FRWheelCollider.brakeTorque = currentBreakForce;
        BLWheelCollider.brakeTorque = currentBreakForce;
        BRWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        FLWheelCollider.steerAngle = currentSteeringAngle;
        FRWheelCollider.steerAngle = currentSteeringAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(FLWheelCollider, FLWheelTransform);
        UpdateSingleWheel(FRWheelCollider, FRWheelTransform);
        UpdateSingleWheel(BLWheelCollider, BLWheelTransform);
        UpdateSingleWheel(BRWheelCollider, BRWheelTransform);
    }

    private void ApplyDownforce()
    {
        rb.AddForce(-transform.up * downforceMultiplier * rb.velocity.magnitude); // Adds downforce based on speed
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}