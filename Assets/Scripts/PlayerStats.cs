using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float ammo;

    public bool hasArmor;
    public float armor;

    public WeaponTypes equippedWeapon;

    public int deaths;
    public int kills;
    public int score;

    // Start is called before the first frame update
    void Awake()
    {
        equippedWeapon = WeaponTypes.NoWeapon;
        health = 100f;
        armor = 0;
        Debug.Log("Set player stats");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
