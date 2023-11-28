using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;

    private float minLength;
    private float maxLength;
    private float springLength;
    private float springForce;

    private Vector3 suspensionForce;

    [Header("Wheel")]
    public float wheelRadius;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, maxLength + wheelRadius))
        {
            springLength = hit.distance - wheelRadius;

            springForce = springStiffness * (restLength - springLength);

            suspensionForce = springForce * transform.up;

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }
}
