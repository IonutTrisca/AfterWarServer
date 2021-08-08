using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesertEagle : SingleShotWeapon
{
    // Start is called before the first frame update
    void Start()
    {
        this.damage = 3f;
        this.range = 100f;
        this.magazineSize = 7;

        fpsCam = Utils.RecursiveFindChild(transform.root, "MainCamera");
        animator = transform.root.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponentInParent<Player>().keysPressed[6])
        {
            Shoot();
        }

        if (currentMagazine == 0 && ammo != 0)
        {
            Debug.Log("Reloading automatically");
            Reload();
        }

    }
}
