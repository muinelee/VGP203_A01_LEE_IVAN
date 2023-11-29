using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target the camera should follow (your car)
    public Vector3 offsetPosition; // The positional offset from the target
    public float followSpeed = 5f; // How quickly the camera catches up to the target's position
    public float rotationSpeed = 5f; // How quickly the camera matches the target's rotation

    private void LateUpdate()
    {
        // Calculate the desired position with the offset
        Vector3 desiredPosition = target.TransformPoint(offsetPosition);

        // Smoothly interpolate between the camera's current position and the desired position
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Calculate the desired rotation to look at the target
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);

        // Smoothly interpolate between the camera's current rotation and the desired rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
