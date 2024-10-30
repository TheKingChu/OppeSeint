using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;

    private float lastShootTime;

    public void ShootButton()
    {
        if(Time.time >= lastShootTime + shootCooldown)
        {
            Shoot();
            lastShootTime = Time.time;
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();
        if(rb2d != null)
        {
            rb2d.velocity = shootPoint.right * (projectile.GetComponent<Projectile>().speed);
        }
    }
}
