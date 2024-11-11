using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int health = 1;
    public int damageToPlayer = 1;
    public float moveSpeed = 2f;
    public bool isMoving = false;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float projectileLifetime = 5f;
    [SerializeField] private float projectileSpeed = 5f;
    private bool hasShot = false;

    [Header("Ground detection")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.2f;

    [Header("Drop Settings")]
    public GameObject healthDrop;
    public GameObject boostDrop;
    [SerializeField] private float dropChance = 0.1f; //10% chance of drop

    private Rigidbody2D rb2d;
    private Transform player;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (isMoving)
        {
            Patrole();
        }

        if(player != null && Vector2.Distance(transform.position, player.position) <= detectionRange)
        {
            Shoot();
        }
    }

    private void Patrole()
    {
        rb2d.velocity = new Vector2(-moveSpeed, rb2d.velocity.y);
    }

    private void Shoot()
    {
        if(!hasShot && projectilePrefab != null && projectileSpawnPoint != null)
        {
            hasShot = true;

            GameObject projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            projectile.AddComponent <EnemyProjectile>().Initialize(damageToPlayer, player, projectileSpeed, projectileLifetime);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
        }
    }
    
    private void TakeDamage()
    {
        health--;

        if(health <= 0)
        {
            DropItem();
            Destroy(gameObject);
        }
    }

    private void DropItem()
    {
        float chance = Random.Range(0f, 1f);

        if(chance <= dropChance)
        {
            GameObject dropPrefab = Random.Range(0f, 1f) > 0.5f ? healthDrop : boostDrop; //50/50 chance to get either
            Instantiate(dropPrefab, transform.position, Quaternion.identity);
        }
    }
}

public class EnemyProjectile : MonoBehaviour
{
    private int damage;
    private float speed;
    private float lifetime;
    private Vector2 direction;

    public void Initialize(int damageToPlayer, Transform target, float speed, float lifetime)
    {
        this.damage = damageToPlayer;
        this.speed = speed;
        this.lifetime = lifetime;

        direction = (target.position - transform.position).normalized;
        StartCoroutine(DestroyAfterLifetime());
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterLifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
