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

    public float muzzleFlashTime;
    float startTime;
    GameObject flash;

    void Start()
    {
        flash = GameObject.Find("Muzzle Flash");
        flash.SetActive(false);
        gunshotSound = GetComponent<AudioSource>();
        originalPitch = gunshotSound.pitch;
    }

    void Update()
    {
        if (flash.activeSelf == true)
        {
            startTime += Time.deltaTime;
            if (startTime > muzzleFlashTime)
            {
                flash.SetActive(false);
            }
        }
    }

    public void Shoot(Animator anim)
    {
        if (Time.time > nextShotTime)
        {
            nextShotTime = Time.time + 1 / fireRate;

            Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation) as Projectile;
            projectile.speed = projectileSpeed;

            flash.SetActive(true);
            startTime = 0;

            gunshotSound.pitch = Random.Range(originalPitch - 0.03f, originalPitch + 0.03f);
            gunshotSound.Play();            
            anim.SetTrigger("onFire");
        }        
    }
}
