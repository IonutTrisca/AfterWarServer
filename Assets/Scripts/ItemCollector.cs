using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public PlayerStats stats;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ItemSpawner")
        {
            other.GetComponent<ItemSpawner>().Collided(this);
        }
        if (other.tag == "Weapon" && !stats.hasWeapon)
        {
        }
    }
}
