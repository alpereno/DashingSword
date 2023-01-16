using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    //[SerializeField] private float dashForceAmount = 10;
    //public event System.Action<float> OnDashing;

    Rigidbody playerRb;
    Vector3 velocity;

    //[Header("Dashing")]
    //[SerializeField] private float dashTime = .4f;
    //[SerializeField] private float dashMultiplier = 2.5f;
    //[SerializeField] private float msBetweenDash = 2000;
    bool dashing;
    //float nextDashTime;
    //AnimatorManager animm;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        //animm = GetComponent<AnimatorManager>();
    }

    void FixedUpdate()
    {
        playerRb.MovePosition(playerRb.position + velocity * Time.fixedDeltaTime);
    }

    public void SetVelocity(Vector3 _velocity)
    {
        //if (!dashing)
        //{
        //    velocity = _velocity;
        //}
        velocity = _velocity;
    }

    //public void Dash(Vector3 direction)
    //{
    //    //playerRb.AddForce(direction * dashForceAmount, ForceMode.Impulse);

    //    // U can make effects with trail renderer
    //    if (Time.time > nextDashTime)
    //    {
    //        if (direction.sqrMagnitude > 0)
    //        {
    //            dashing = true;
    //            nextDashTime = Time.time + msBetweenDash / 1000f;
    //            velocity *= dashMultiplier;

    //            Invoke("DisableDashing", dashTime);
    //            animm.Dash(dashing);

    //            if (OnDashing != null)
    //            {
    //                OnDashing(dashTime);
    //            }
    //        }
    //    }
    //}

    public void Dash(float dashAmount)
    {
        velocity *= dashAmount;
    }

    //// disable dashing effects
    //public void DisableDashing()
    //{        
    //    dashing = false;
    //}
}
