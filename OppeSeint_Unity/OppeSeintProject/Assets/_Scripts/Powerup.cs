using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float newShootCooldown = 0.2f;
    public float duration = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerShooting playerShooting = collision.GetComponent<PlayerShooting>();
            if(playerShooting != null)
            {
                playerShooting.ActivatePowerUp(newShootCooldown, duration);
            }

            Destroy(gameObject);
        }
    }
}