using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Transform attackPointTransform;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask hittableObjectMask;
    [SerializeField] private float damage = 1;

    // The Sword is applying damage to enemies there...
    // Does damage the target object(s)
    public void ApplyDamage()
    {
        Collider[] hitObjects = Physics.OverlapSphere(attackPointTransform.position, attackRange, 
            hittableObjectMask, QueryTriggerInteraction.Collide);
        foreach (Collider hitCollider in hitObjects)
        {
            IDamageable damageableObject = hitCollider.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(damage, attackPointTransform.position);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (attackPointTransform != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(attackPointTransform.position, attackRange);
        }
    }
}
