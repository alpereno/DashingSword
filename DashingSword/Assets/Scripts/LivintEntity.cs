using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivintEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private float startingHealth = 3;
    [SerializeField] protected float health;
    protected bool dead;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void TakeDamage(float damage, Vector3 hitPoint)
    {
        print("Taking damage = " + damage + " from = " + hitPoint);
    }
}
