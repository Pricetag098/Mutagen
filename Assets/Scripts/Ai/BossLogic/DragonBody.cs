using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBody : MonoBehaviour
{
    public Transform target;
    public float followDistance = 2f;
    public float smoothSpeed = 5f;
    public float rotationSpeed = 5f;
    public float spiralSpeed;
    public float followDelay = 0.5f;

    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Quaternion desiredRotation;
    private Vector3 smoothVelocity;

    private void LateUpdate()
    {
        targetPosition = target.position - (target.forward * followDistance);
        targetRotation = target.rotation;

        
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothVelocity, smoothSpeed, followDelay);

        //this ma
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotationWithoutY = Quaternion.Euler(currentRotation.eulerAngles.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        Quaternion targetRotationWithoutYZ = Quaternion.Euler(targetRotationWithoutY.eulerAngles.x, 0f, 0f);
        desiredRotation = Quaternion.Lerp(currentRotation, targetRotationWithoutYZ, rotationSpeed * Time.deltaTime);

        //this makes the objects rotate in the z axis
        float desiredZRotation = Mathf.LerpAngle(currentRotation.eulerAngles.z, targetRotationWithoutY.eulerAngles.z, spiralSpeed * Time.deltaTime);
        desiredRotation.eulerAngles = new Vector3(desiredRotation.eulerAngles.x, desiredRotation.eulerAngles.y, desiredZRotation);

        transform.rotation = desiredRotation;

        // Look at the target
        transform.rotation = Quaternion.LookRotation(targetPosition);
    }
}
