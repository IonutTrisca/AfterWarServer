using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;

    public Transform shootOrigin;
    public float spineAngle;

    public PlayerStats stats;

    public bool[] keysPressed;
    public bool isGrounded;

    public void Initialize(int id, string username)
    {
        this.id = id;
        this.username = username;
        keysPressed = new bool[7];
    }

    public void FixedUpdate()
    {
        if (stats.health <= 0f)
        {
            return;
        }

        ServerSend.PlayerMovement(this);
        Debug.DrawRay(shootOrigin.position, shootOrigin.forward * 2, Color.red);
    }

    public void SetKeysPressed(bool[] keys)
    {
        for (int i = 0; i < keys.Length; i++)
        {
            keysPressed[i] = keys[i];
        }
    }

    /*public void Shoot(Vector3 viewDirection)
    {
        if (health <= 0f)
        {
            return;
        }

        if (Physics.Raycast(shootOrigin.position, viewDirection, out RaycastHit hit, 25f))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.GetComponent<Player>().TakeDamage(20f))
                {
                    kills++;
                    score += 20;
                    //ServerSend.PlayerEliminated(id, Server.clients[hit.collider.GetComponent<Player>().id].id);
                }
                else
                {
                    score += 2;
                }

                //ServerSend.PlayerStats(this);
            }
        }
    }*/

    /*public bool TakeDamage(float damage)
    {
        if (health <= 0f)
        {
            return false;
        }

        health -= damage;

        if (health <= 0f)
        {
            health = 0f;
            deaths++;
            return true;
        }

        // ServerSend.PlayerHealth(this);
        return false;
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        kills = 0;
        deaths = 0;
        score = 0;
    }*/
}
