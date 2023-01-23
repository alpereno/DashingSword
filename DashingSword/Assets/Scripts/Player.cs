using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimatorManager))]
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public enum State { Movement, Attack, Idle };
    State currentState;

    public event Action<float> OnDashing;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 1.5f;
    [SerializeField] private float runSpeed = 5;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 15;

    [Header("Dashing")]
    [SerializeField] private float dashTime = .4f;              // How long is the dashing move
    [SerializeField] private float dashMultiplier = 2.5f;
    [SerializeField] private float msBetweenDash = 2000;        // Time that must pass between each dashing move (in miliseconds)
    bool dashing;

    [Header("Attack")]
    [SerializeField] private float msBetweenAttacks = 5000;
    bool isAttackingContinue;

    Transform mainCam;
    PlayerController playerController;
    SwordController swordController;
    AnimatorManager animatorManager;
    CameraManager cameraManager;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animatorManager = GetComponent<AnimatorManager>();
        swordController = GetComponent<SwordController>();
        mainCam = Camera.main.transform;
        cameraManager = mainCam.GetComponentInParent<CameraManager>();

        currentState = State.Movement;
    }

    void Update()
    {
        MovementInput();
        AttackInput();
        MouseInput();
    }

    private void MovementInput()
    {
        if (currentState == State.Movement)
        {
            Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            float moveAmount = Mathf.Clamp01(Mathf.Abs(moveInput.x) + Mathf.Abs(moveInput.z));
            animatorManager.UpdateAnimatorValues(0, moveAmount);

            Vector3 moveVelocity = mainCam.forward * moveInput.z;
            moveVelocity += mainCam.right * moveInput.x;
            moveVelocity = moveVelocity.normalized;
            moveVelocity.y = 0;

            // Dashing Input
            // The Dashing can only be when the player on moving
            if (Input.GetKeyDown(KeyCode.E))
            {
                dashing = playerController.Dash(moveVelocity, dashMultiplier, msBetweenDash);

                if (dashing)
                {
                    animatorManager.UpdateDashValue(dashing);
                    Invoke("DisableDashing", dashTime);

                    if (OnDashing != null)
                    {
                        OnDashing(dashTime);
                    }
                }
            }

            playerController.HandleRotation(moveVelocity, rotationSpeed);

            moveVelocity *= (moveAmount > .5f) ? runSpeed : walkSpeed;

            //FPS game move direction (relative to local coordinate system)
            //moveVelocity = transform.TransformDirection(moveVelocity);
            if (!dashing)
            {
                playerController.SetVelocity(moveVelocity);
            }
        }
    }

    private void MouseInput()
    {
        Vector2 cameraInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        cameraManager.SetCameraInputValues(cameraInput);
    }

    private void AttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //// U can check if the player has a sword (it could be depends of sword controller script's a bool property)
            if (!isAttackingContinue)
            {
                isAttackingContinue = true;
                currentState = State.Attack;
                swordController.Attack(msBetweenAttacks);
                playerController.SetVelocity(Vector3.zero);
                animatorManager.AnimationSetBool("Moving", false);                
                float attackTime = animatorManager.PlayAttackAnimation();
                Invoke("DisableAttacking", attackTime);
            }
        }
    }

    private void DisableDashing()
    {
        dashing = false;
        animatorManager.UpdateDashValue(dashing);
    }

    private void DisableAttacking()
    {
        isAttackingContinue = false;
        currentState = State.Movement;
        animatorManager.AnimationSetBool("Moving", true);
    }
}
