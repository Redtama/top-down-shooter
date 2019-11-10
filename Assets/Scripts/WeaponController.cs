using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHold;
    public Transform weaponArm;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;
    public float minAimDistance;

    private Weapon equippedWeapon;
    private Animator playerAnimator;
    private float nextFire;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        EquipWeapon(primaryWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = Instantiate(weapon, weaponHold);
    }

    public void Shoot()
    {
        if (equippedWeapon != null && Time.time > nextFire)
        {
            nextFire = Time.time + 1 / equippedWeapon.fireRate;
            equippedWeapon.Shoot();
            playerAnimator.SetTrigger("onFire");
        }
    }

    void Update()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimPos = mousePos;
        float sqrMouseDistance = ((Vector2)(mousePos - transform.position)).sqrMagnitude;

        if (sqrMouseDistance < Mathf.Pow(minAimDistance, 2))
        {
            aimPos = transform.position + transform.up * minAimDistance;
        }

        weaponArm.rotation = Quaternion.LookRotation(Vector3.forward, aimPos - weaponArm.position);
    }
}
