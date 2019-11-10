using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHandler : MonoBehaviour
{
    public int health;
    public float colorChangeDuration;

    Rigidbody2D rb;
    SpriteRenderer[] sprites;
    float colorChangeElapsedTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (health <= 0)
        {
            Dead();
        }

        colorChangeElapsedTime += Time.deltaTime;
        if (colorChangeElapsedTime > colorChangeDuration)
        {
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = Color.white;
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        health -= damage;

        foreach (SpriteRenderer sprite in sprites)
        {
            sprite.color = Color.red;            
        }
        colorChangeElapsedTime = 0;
    }

    void Dead()
    {
        rb.isKinematic = false;
        Destroy(gameObject);
    }

    void FlashRed()
    {

    }
}
