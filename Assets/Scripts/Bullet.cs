using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Weapon
{
    public float destroyTime;
    public Rigidbody2D rb;
    public float bulletSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
        rb.velocity = transform.up * bulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Debug.Log(hitInfo.name);
        Destroy(gameObject);
    }
}
