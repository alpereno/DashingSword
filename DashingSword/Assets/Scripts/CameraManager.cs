using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] Transform cameraPivot;     // The object the camera uses to pivot
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraFollowSpeed = .2f;
    [SerializeField] private float cameraLookSpeed = 2;
    [SerializeField] private float cameraPivotSpeed = 2;
    [SerializeField] private Vector2 pivotAngleMinMax;

    private Vector3 cameraFollowVelocity;
    float mouseInputX;
    float mouseInputY;

    public float lookAngle;     // Camera looking up and down
    public float pivotAngle;    // Camera looking left and right

    private void Start()
    {
        targetTransform = FindObjectOfType<Player>().transform;
    }

    void LateUpdate()
    {
        FollowTarget();
        RotateCamera();
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    public void RotateCamera()
    {
        lookAngle = lookAngle + (mouseInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (mouseInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, pivotAngleMinMax.x, pivotAngleMinMax.y);

        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    public void SetCameraInputValues(Vector2 mouseInput)
    {
        mouseInputX = mouseInput.x;
        mouseInputY = mouseInput.y;
    }
}
