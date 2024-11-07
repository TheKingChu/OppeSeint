using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    public int damageToPlayer = 2;

    public Transform bossTransform;
    [SerializeField] private float floatRadius = 2f;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] float detectionRange = 10f;
    [SerializeField] float projectileSpeed = 2f;

    private Transform player;
    private bool isLaunched = false;
    private Vector2 originalOffset; //track position around the boss

    private void Start()
    {
        if(bossTransform == null)
        {
            Debug.LogError("No boss transform found");
            return;
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalOffset = transform.position - bossTransform.position;
    }

    private void Update()
    {
        if (!isLaunched)
        {
            float angle = rotateSpeed * Time.deltaTime;
            transform.position = bossTransform.position + (Vector3)(Quaternion.Euler(0, 0, angle) * originalOffset);

            if(player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
            {
                LauchProjectile();
            }
        }
    }

    private void LauchProjectile()
    {
        isLaunched = true;

        Vector2 direction = (player.position - transform.position).normalized;

        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if(rb2d != null)
        {
            rb2d.velocity = direction * projectileSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
