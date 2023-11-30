using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Wheels")]
    public Transform FLWheelSteer;
    public Transform FRWheelSteer;

    public Transform FLWheelSpin;
    public Transform FRWheelSpin;
    public Transform BLWheelSpin;
    public Transform BRWheelSpin;

    public float wheelSpinSpeed = 100.0f;

    [Header("Car Stats")]
    public float forwardAccel = 15.0f;
    public float reverseAccel = 5.0f;
    //public float maxSpeed = 200.0f;
    public float turnStrength = 100.0f;
    public float wheelTurnAngle = 40.0f;
    public float dragOnGround = 3.0f;

    [Header("Ground Raycast")]
    public LayerMask whatIsGround;
    public float groundRayLength = 0.5f;
    public Transform groundRayPoint;

    public float gravityForce = 15.0f;
    private bool grounded;

    [Header("Inputs")]
    public float gasInput;
    public float steerInput;

    [Header("Input References")]
    private float speedInput;
    private float turnInput;
    private float lastDirection = 1.0f;

    void Start()
    {
        rb.transform.parent = null;
    }

    void Update()
    {
        speedInput = 0.0f;

        if (gasInput > 0)
        {
            speedInput = gasInput * forwardAccel * 1000.0f;
            lastDirection = 1.0f;
        }
        else if (gasInput < 0)
        {
            speedInput = gasInput * reverseAccel * 1000.0f;
            lastDirection = -1.0f;
        }

        // Apply visual spin to all wheels
        float wheelRotationAmount = rb.velocity.magnitude * wheelSpinSpeed * Time.deltaTime;
        float rotationDirection = speedInput != 0 ? (speedInput >= 0 ? 1.0f : -1.0f) : lastDirection;

        RotateWheels(FLWheelSpin, wheelRotationAmount * rotationDirection);
        RotateWheels(FRWheelSpin, wheelRotationAmount * rotationDirection);
        RotateWheels(BLWheelSpin, wheelRotationAmount * rotationDirection);
        RotateWheels(BRWheelSpin, wheelRotationAmount * rotationDirection);

        //turnInput = Input.GetAxis("Horizontal");
        turnInput = steerInput;

        if (grounded)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, turnInput * turnStrength * Time.deltaTime * gasInput, 0.0f));

            float wheelAngle = turnInput * wheelTurnAngle;
            FLWheelSteer.localEulerAngles = new Vector3(FLWheelSteer.localEulerAngles.x, wheelAngle, FLWheelSteer.localEulerAngles.z);
            FRWheelSteer.localEulerAngles = new Vector3(FRWheelSteer.localEulerAngles.x, wheelAngle, FRWheelSteer.localEulerAngles.z);
        }

        transform.position = rb.transform.position;
    }

    // Visual spin of the wheels
    void RotateWheels(Transform wheel, float rotationAmount)
    {
        wheel.Rotate(rotationAmount, 0, 0, Space.Self);
    }

    private void FixedUpdate()
    {
        grounded = false;
        RaycastHit hit;

        // Allow the car to steer if grounded
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        // Apply drag when on the ground
        if (grounded)
        {
            rb.drag = dragOnGround;

            if (Mathf.Abs(speedInput) > 0)
            {
                rb.AddForce(transform.forward * speedInput);
            }
        }
        // Apply gravity when in the air
        else
        {
            rb.drag = 0.1f;

            rb.AddForce(Vector3.up * -gravityForce * 100f);
        }
    }
}
