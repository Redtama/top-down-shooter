using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnHit : MonoBehaviour
{    
    public Weapon weapon;

    void OnTriggerEnter2D(Collider2D col)
    {
        HealthHandler health = col.GetComponent<HealthHandler>();

        if (health != null)
        {
            health.ApplyDamage(weapon.damage);
        }              

        Destroy(gameObject);
    }
}
