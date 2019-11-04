using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHold;
    public Weapon primaryWeapon;
    public Weapon secondaryWeapon;

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
}
