using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector]
    public float speed;

    private Rigidbody2D rb;
    Vector3 mousePos;
    Vector2 bulletDirection;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bulletDirection = mousePos - transform.position;
        bulletDirection.Normalize();        
    }

    // Update is called once per frame
    void Update()
    {        
        rb.velocity = bulletDirection * speed;
    }
}
