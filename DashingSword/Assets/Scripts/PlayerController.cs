using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //public event System.Action<float> OnDashing;

    Rigidbody playerRb;
    Vector3 velocity;

    float nextDashTime;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + velocity * Time.fixedDeltaTime);
    }

    public void SetVelocity(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void HandleRotation(Vector3 targetDirection, float rotationSpeed)
    {
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
            targetDirection = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public bool Dash(Vector3 moveVelocity, float dashAmount, float msBetweenDash)
    {
        if (Time.time > nextDashTime)
        {
            if (moveVelocity.sqrMagnitude <= 0) return false; // it wont be less then 0 cause its sqrMagnitde

            nextDashTime = Time.time + msBetweenDash / 1000;
            velocity *= dashAmount;
            return true;
        }
        else return false;
    }
}
