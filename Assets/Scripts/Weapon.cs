using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public Projectile projectilePrefab;
    public float fireRate;
    public float projectileSpeed;
    public int damage;

    private float nextShotTime;
    private AudioSource gunshotSound;
    private float originalPitch;

    void Start()
    {
        gunshotSound = GetComponent<AudioSource>();
        originalPitch = gunshotSound.pitch;
    }

    public void Shoot(Animator anim)
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1 / fireRate;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Quaternion fireRotation = Quaternion.LookRotation(Vector3.forward, mousePos - firePoint.position);

            Projectile projectile = Instantiate(projectilePrefab, firePoint.position, fireRotation) as Projectile;
            projectile.speed = projectileSpeed;

            gunshotSound.pitch = Random.Range(originalPitch - 0.03f, originalPitch + 0.03f);
            gunshotSound.Play();            
            anim.SetTrigger("onFire");
        }        
    }    
}
