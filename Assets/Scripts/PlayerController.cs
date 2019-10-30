﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] // All this does is tell Unity to put a Heading in the inspector. (Helps organise our variables in the inspector a bit.)
    public float speed;
    public float blinkSpeed;

    [Header("Weapon")]
    public Weapon weapon;
    public Transform firePoint;

    [HideInInspector]
    public Vector2 input;

    Vector2 movement;
    float nextFire = 0;

    private Rigidbody2D rb;
    private RaycastHit2D[] hitResults = new RaycastHit2D[16];
    private float skinWidth = 0.01f;


    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController Initialised");
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }

    
    void UpdateMovement()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movement = input * speed * Time.fixedDeltaTime;

        if (movement.magnitude != 0)
        {
            CollisionHandler.HandleCollisions(rb, ref movement, skinWidth);
            transform.Translate(movement, Space.World);                      
        }        

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }

    void UpdateShooting()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + 1 / weapon.fireRate;
            weapon.Shoot(firePoint);
        }
    }
}
