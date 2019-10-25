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

    Vector2 input;
    Vector2 movement;
    float nextFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController Initialised");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        UpdateShooting();
    }

    void UpdateMovement()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement = input * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (Input.GetKeyDown("space"))
        {
            transform.Translate(movement * blinkSpeed, Space.World);
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
