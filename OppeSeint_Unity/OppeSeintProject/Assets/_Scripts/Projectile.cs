using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifeTime = 2f;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = transform.right * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Boss"))
        {
            BossScript bossScript = collision.gameObject.GetComponent<BossScript>();
            if (bossScript != null)
            {
                bossScript.TakeDamage(1);
            }

            // Stop the projectile from pushing the boss
            Rigidbody2D projectileRb = GetComponent<Rigidbody2D>();
            if(projectileRb != null)
            {
                projectileRb.velocity = Vector2.zero;
                projectileRb.bodyType = RigidbodyType2D.Kinematic;
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("BossProjectiles"))
        {
            // Stop the projectiles from moving on collision
            Rigidbody2D bossProjectileRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (bossProjectileRb != null)
            {
                bossProjectileRb.velocity = Vector2.zero; // Nullify any movement caused by the collision
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
