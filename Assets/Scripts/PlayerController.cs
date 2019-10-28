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
        movement = input * speed * Time.deltaTime;

        if (movement.magnitude > 0.001f)
        {
            HandleCollisions();
            transform.Translate(movement, Space.World);
        }        

        if (Input.GetKeyDown("space"))
        {
            transform.Translate(movement * blinkSpeed, Space.World);
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
    }

    void HandleCollisions()
    {
        float directionY = Mathf.Sign(movement.y);
        float moveDistance = movement.magnitude;
        float castDistance = moveDistance + skinWidth;

        int hitCount = rb.Cast(movement.normalized, hitResults, castDistance);


        for (int i = 0; i < hitCount; i++)
        {            
            RaycastHit2D hit = hitResults[i];

            float projection = Vector2.Dot(movement.normalized, hit.normal);
            float slopeDirectionX = Mathf.Sign(hit.normal.x);
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.right * slopeDirectionX);

            movement.x = moveDistance * Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * slopeDirectionX;
            movement.y = moveDistance * Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * directionY;                        
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
