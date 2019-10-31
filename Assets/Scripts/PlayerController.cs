using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionHandler))]
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
    private CollisionHandler collisionHandler;
    private AudioSource gunshot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collisionHandler = GetComponent<CollisionHandler>();
        gunshot = GetComponent<AudioSource>();
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

        if (movement.magnitude != 0)
        {
            collisionHandler.HandleCollisions(rb, ref movement, skinWidth);
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
            gunshot.pitch = Random.Range(.95f, 1.05f);
            gunshot.Play();
        }
    }
}
