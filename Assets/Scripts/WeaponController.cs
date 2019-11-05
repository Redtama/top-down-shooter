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

    [SerializeField]
    private Weapon equippedWeapon;

    void Start()
    {
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

    public void Shoot(Animator anim)
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.Shoot(anim);
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
