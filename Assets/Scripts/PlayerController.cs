using System;
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
            CheckCollision();
            transform.Translate(movement, Space.World);                      
        }        

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }

    void CheckCollision()
    {
        float castDistance = movement.magnitude + skinWidth;

        int hitCount = rb.Cast(movement.normalized, hitResults, castDistance);
        
        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];

            Vector2 slopeNormal = hit.normal;
            Vector2 slopePathTest = Vector2.Perpendicular(hit.normal);
            if (Vector2.Dot(movement, slopePathTest) > 0)
            {
                movement = slopePathTest * Vector2.Dot(movement.normalized, slopePathTest) * movement.magnitude;                    
            }
            else
            {
                movement = -slopePathTest * Vector2.Dot(movement.normalized, -slopePathTest) * movement.magnitude;                    
            }
        }        
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
