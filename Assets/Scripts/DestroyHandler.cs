using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyHandler : MonoBehaviour
{
    public float lifetime;
    int damage;
    public Weapon weapon;

    void Awake()
    {
        damage = weapon.damage;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject collidedObject = GameObject.Find(col.name);
        HealthHandler health = collidedObject.GetComponent<HealthHandler>();
        if (health != null)
        {
            health.ApplyDamage(damage);
        }              
        GameObject.Destroy(gameObject);
    }
}
