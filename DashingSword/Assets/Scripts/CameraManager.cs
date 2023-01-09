using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraFollowSpeed = .2f;

    private Vector3 cameraFollowVelocity;

    private void Start()
    {
        targetTransform = FindObjectOfType<Player>().transform;
    }

    void LateUpdate()
    {
        FollowTarget();
    }

    public void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }
}
