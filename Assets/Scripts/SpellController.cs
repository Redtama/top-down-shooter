using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    public float cooldown;
    public Projectile spell;
    public Transform firePoint;
    public float spellSpeed;

    private Animator playerAnimator;
    private float nextFire;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CastSpell()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + cooldown;
            Projectile projectile = Instantiate(spell, firePoint.position, firePoint.rotation) as Projectile;
            projectile.speed = spellSpeed;
            playerAnimator.SetTrigger("onSpell");
        }
    }
}
