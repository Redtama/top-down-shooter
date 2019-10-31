using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public int health;

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
        Destroy(gameObject);
    }
}
