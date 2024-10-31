using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootCooldown = 0.5f;
    public SpriteRenderer gunRenderer;

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
        AdjustShootPointPosition();
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();
        if(rb2d != null)
        {
            Vector2 shootDirection = gunRenderer.flipX ? Vector2.left : Vector2.right;
            rb2d.velocity = shootDirection * projectile.GetComponent<Projectile>().speed;
        }
    }

    private void AdjustShootPointPosition()
    {
        // Set the shoot point's position based on gun orientation
        if (gunRenderer.flipX)
        {
            shootPoint.localPosition = new Vector3(-0.5f, 0, 0); // Left orientation
            shootPoint.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            shootPoint.localPosition = new Vector3(0.5f, 0, 0); // Right orientation
            shootPoint.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
