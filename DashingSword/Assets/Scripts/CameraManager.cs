using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot;         // The object the camera uses to pivot.
    Transform targetTransform;     // The object the camera will follow.
    Transform cameraTransform;     // The transform of the actual camera object.
    Camera mainCamera;
    Player player;

    [Header("Camera Follow Variables")]
    [SerializeField] private float cameraFollowSpeed = .2f;
    [SerializeField] private float cameraLookSpeed = 2;
    [SerializeField] private float cameraPivotSpeed = 2;
    [SerializeField] private Vector2 pivotAngleMinMax;

    [Header("Camera Collisions")]
    [SerializeField] private float cameraCollisionRadius = 2;
    [SerializeField] private LayerMask collisionLayers;         // The layers we want to our camera collide with.
    [SerializeField] private float cameraCollisionOffset = .2f; // How much the camera will jump off of objects its colliding with.
    [SerializeField] private float minimumCollisionOffset = .2f;

    Vector3 cameraFollowVelocity = Vector3.zero;
    Vector3 cameraVectorPosition;

    float mouseInputX;
    float mouseInputY;
    float defaultPosition;

    float lookAngle;     // Camera looking up and down
    float pivotAngle;    // Camera looking left and right
    
    // Dashing
    float fovDashTime;
    float dashingFOVValue = 75;

    void Start()
    {
        player = FindObjectOfType<Player>();
        targetTransform = player.transform;
        player.OnDashing += DashingFOV;         // Event Subscribe
        mainCamera = Camera.main;
        cameraTransform = mainCamera.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void LateUpdate()
    {
        RotateCamera();
        CheckCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;

        //transform.position = Vector3.Lerp(transform.position, targetTransform.position, cameraFollowSpeed);
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

    private void DashingFOV(float dashTime)
    {
        fovDashTime = dashTime;
        StartCoroutine(DashingFOV());
    }

    IEnumerator DashingFOV()
    {
        float startingValue = mainCamera.fieldOfView;
        float endValue = dashingFOVValue;
        float increaseSpeed = 1 / (fovDashTime * 2.5f); // 1 
        float percent = 0;

        while (percent <= 1)
        {
            percent += increaseSpeed * Time.deltaTime;
            //percent should be start pos go to max pos and BACK to start pos so value be 0 to 1 and back to 0 again
            //when x=0 y=0, x=1 y=0, x=1/2 y=1  so solve the inspired by y=a(x-x1)(x-x2) equations a=-4
            //so parabola equation working y = 4(-x^2+x)            when percent 1/2 value is 1
            // 4(-percent*percent + percent)
            float interpolation = 4 * (-percent * percent + percent);
            mainCamera.fieldOfView = Mathf.Lerp(startingValue, endValue, interpolation);
            yield return null;
        }
    }
}
