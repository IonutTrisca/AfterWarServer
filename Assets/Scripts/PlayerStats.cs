using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float ammo;

    public bool hasArmor;
    public float armor;

    public bool hasWeapon;

    public int deaths;
    public int kills;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        armor = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
