using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int damageToPlayer = 2;
    public BossScript bossScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealt = collision.gameObject.GetComponent<PlayerHealth>();
            if(playerHealt != null)
            {
                playerHealt.TakeDamage(damageToPlayer);
            }
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
        else if(collision.gameObject.CompareTag("Projectile"))
        {
            gameObject.SetActive(false);
        }

        if(bossScript != null)
        {
            bossScript.CheckAllProjectilesDestroyed();
        }
    }

}
