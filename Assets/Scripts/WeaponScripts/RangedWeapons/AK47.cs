using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : AutomaticWeapon
{
    // Start is called before the first frame update
    void Awake()
    {
        this.damage = 3f;
        this.range = 100f;
        this.fireRate = 12f;
        this.nextTimeToFire = 0f;
        this.magazineSize = 30;
        this.ammo = 100;
        type = WeaponTypes.AK47;

        fpsCam = Utils.RecursiveFindChild(transform.root, "MainCamera");
        animator = transform.root.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<Player>().keysPressed[6] && Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
        }

        if (currentMagazine == 0 && ammo != 0)
        {
            Debug.Log("Reloading automatically");
            Reload();
        }
    }
}
