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
    public float muzzleFlashDuration;

    private AudioSource gunshotSound;
    private float originalPitch;
    float muzzleFlashElapsedTime;
    GameObject muzzleFlash;

    void Start()
    {
        muzzleFlash = transform.Find("Muzzle Flash").gameObject;
        muzzleFlash.SetActive(false);
        gunshotSound = GetComponent<AudioSource>();
        originalPitch = gunshotSound.pitch;
    }

    void Update()
    {
        if (muzzleFlash.activeSelf == true)
        {
            muzzleFlashElapsedTime += Time.deltaTime;
            if (muzzleFlashElapsedTime > muzzleFlashDuration)
            {
                muzzleFlash.SetActive(false);
            }
        }
    }

    public void Shoot()
    {
        Projectile projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation) as Projectile;
        projectile.speed = projectileSpeed;

        muzzleFlash.SetActive(true);
        muzzleFlashElapsedTime = 0;

        gunshotSound.pitch = Random.Range(originalPitch - 0.03f, originalPitch + 0.03f);
        gunshotSound.Play();
    }
}
