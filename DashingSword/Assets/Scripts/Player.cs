using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AnimatorManager))]
[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 4;
    [SerializeField] private float runSpeed = 6;
    [SerializeField] private float rotationSpeed = 15;

    Transform mainCam;
    PlayerController playerController;
    AnimatorManager animatorManager;
    CameraManager cameraManager;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animatorManager = GetComponent<AnimatorManager>();
        mainCam = Camera.main.transform;
        cameraManager = mainCam.GetComponentInParent<CameraManager>();
    }

    void Update()
    {
        MovementInput();        
    }

    private void LateUpdate()
    {
        MouseInput();
    }

    private void MovementInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        float moveAmount = Mathf.Clamp01(Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.z));
        animatorManager.UpdateAnimatorValues(0, moveAmount);

        Vector3 moveVelocity = mainCam.forward * moveInput.z;
        moveVelocity += mainCam.right * moveInput.x;
        moveVelocity = moveVelocity.normalized;
        moveVelocity.y = 0;

        HandleRotation(moveVelocity);

        if (Input.GetKey(KeyCode.LeftShift))
            moveVelocity *= runSpeed;
        else moveVelocity *= walkSpeed;

        //FPS game move direction (relative to local coordinate system)
        //moveVelocity = transform.TransformDirection(moveVelocity);

        playerController.SetVelocity(moveVelocity);
    }

    private void HandleRotation(Vector3 targetDirection)
    {
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void MouseInput()
    {
        Vector2 cameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraManager.SetCameraInputValues(cameraInput);
    }
}
