using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;         // The object the camera uses to pivot.
    [SerializeField] private Transform targetTransform;     // The object the camera will follow.
    [SerializeField] private Transform cameraTransform;     // The transform of the actual camera object.
    [SerializeField] private float cameraFollowSpeed = .2f;
    [SerializeField] private float cameraLookSpeed = 2;
    [SerializeField] private float cameraPivotSpeed = 2;
    [SerializeField] private Vector2 pivotAngleMinMax;
    [SerializeField] private float cameraCollisionRadius = 2;
    [SerializeField] private LayerMask collisionLayers;         // The layers we want to our camera collide with.
    [SerializeField] private float cameraCollisionOffset = .2f; // How much the camera will jump off of objects its colliding with.
    [SerializeField] private float minimumCollisionOffset = .2f;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraVectorPosition;
    float mouseInputX;
    float mouseInputY;
    float defaultPosition;

    public float lookAngle;     // Camera looking up and down
    public float pivotAngle;    // Camera looking left and right

    void Start()
    {
        targetTransform = FindObjectOfType<Player>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    void LateUpdate()
    {
        FollowTarget();
        RotateCamera();
        CheckCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        lookAngle = lookAngle + (mouseInputX * cameraLookSpeed);
        pivotAngle = pivotAngle - (mouseInputY * cameraPivotSpeed);
        pivotAngle = Mathf.Clamp(pivotAngle, pivotAngleMinMax.x, pivotAngleMinMax.y);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void CheckCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction = direction.normalized;

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, 
            Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition = - (distance - cameraCollisionOffset);
        }
        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition = targetPosition - minimumCollisionOffset;
        }
        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, .2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }

    public void SetCameraInputValues(Vector2 mouseInput)
    {
        mouseInputX = mouseInput.x;
        mouseInputY = mouseInput.y;
    }
}
