using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public int health;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Dead();
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;
    }

    void Dead()
    {
        rb.isKinematic = false;
    }
}
