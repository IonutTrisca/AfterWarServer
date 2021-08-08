using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G28Rifle : AutomaticWeapon
{

    // Start is called before the first frame update
    void Awake()
    {
        this.damage = 2f;
        this.range = 150f;
        this.fireRate = 20f;
        this.nextTimeToFire = 0f;
        this.magazineSize = 10;
        type = WeaponTypes.G28;

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
