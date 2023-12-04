using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private Rigidbody rb;
    public WheelColliders wheelColliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    public float brakeInput;
    public float steerInput;

    public float motorPower;
    public float brakePower;
    private float slipAngle;
    private float speed;
    public AnimationCurve steerCurve;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        ApplyWheelPositions();        
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.Angle(transform.forward, rb.velocity - transform.forward);
        if (slipAngle < 120f)
        {
            if (gasInput < 0)
            {
                brakeInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
            else
            {
                brakeInput = 0;
            }
        }
    }

    void ApplyBrake()
    {
        wheelColliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        wheelColliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.7f;
        wheelColliders.BLWheel.brakeTorque = brakeInput * brakePower * 0.3f;
        wheelColliders.BRWheel.brakeTorque = brakeInput * brakePower * 0.3f;
    }

    void ApplyMotor()
    {
        wheelColliders.FLWheel.motorTorque = gasInput * motorPower;
        wheelColliders.FRWheel.motorTorque = gasInput * motorPower;
    }

    void ApplySteering()
    {
        float steerAngle = steerCurve.Evaluate(speed) * steerInput;
        wheelColliders.FLWheel.steerAngle = steerAngle;
        wheelColliders.FRWheel.steerAngle = steerAngle;
    }

    void ApplyWheelPositions()
    {
        UpdateWheel(wheelColliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(wheelColliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(wheelColliders.BLWheel, wheelMeshes.BLWheel);
        UpdateWheel(wheelColliders.BRWheel, wheelMeshes.BRWheel);
    }

    void UpdateWheel(WheelCollider wc, MeshRenderer wm)
    {
        Quaternion quat;
        Vector3 pos;
        wc.GetWorldPose(out pos, out quat);
        wm.transform.position = pos;
        wm.transform.rotation = quat;
    }

    [System.Serializable]
    public class WheelColliders
    {
        public WheelCollider FLWheel;
        public WheelCollider FRWheel;
        public WheelCollider BLWheel;
        public WheelCollider BRWheel;
    }

    [System.Serializable]
    public class WheelMeshes
    {
        public MeshRenderer FLWheel;
        public MeshRenderer FRWheel;
        public MeshRenderer BLWheel;
        public MeshRenderer BRWheel;
    }
}
