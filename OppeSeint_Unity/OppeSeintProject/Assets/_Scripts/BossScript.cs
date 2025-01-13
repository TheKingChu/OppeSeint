using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 1f;
    public Transform pointA;
    public Transform pointB;
    private Transform targetPoint;

    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    public Image heartSpriteRenderer;
    public Sprite fullHealth;
    public Sprite halfHealth;
    public Sprite noHealth;

    [Header("Damage")]
    [SerializeField] private int damageToPlayer = 2;

    [Header("Death Effect")]
    public GameObject explosionEffectPrefab;
    public Camera mainCamera;
    [SerializeField] private float slowMotion = 0.1f;
    [SerializeField] private float zoomInSize = 3f;
    //[SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float explosionDuration = 2f;

    [Header("Win sequence")]
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private Transform chestSpawnPoint;
    [SerializeField] private float sequenceDuration = 7f;
    private bool isSequencePlayer = false;
    public GameObject winCanvas;

    [Header("Projectile")]
    public Transform projectileContainer;
    public List<BossProjectile> projectiles = new List<BossProjectile>();
    [SerializeField] private float projectileSpeed = 2f;
    private int currentProjectileIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        targetPoint = pointB;

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Initialize the projectiles list by finding all projectiles in the child container
        if (projectileContainer != null)
        {
            // Get all children that are projectiles
            for (int i = 0; i < projectileContainer.childCount; i++)
            {
                // Assuming projectiles are of type BossProjectile
                BossProjectile projectile = projectileContainer.GetChild(i).GetComponent<BossProjectile>();
                if (projectile != null)
                {
                    projectiles.Add(projectile);
                }
            }
        }
        else
        {
            Debug.LogError("Projectile container is not assigned in the BossScript.");
        }
    }

    private void Update()
    {
        Patrole();
    }

    private void Patrole()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.4f)
        {
            // Switch target point
            targetPoint = targetPoint == pointA ? pointB : pointA;
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
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthSprite();

        ShootProjectile();

        if (currentHealth <= 0 && !isSequencePlayer)
        {
            StartCoroutine(Death());
        }
    }

    private void ShootProjectile()
    {
        if (projectiles.Count == 0)
        {
            Debug.LogWarning("No projectiles available to fire.");
            return; // Exit if there are no projectiles in the list
        }

        BossProjectile projectile = projectiles[currentProjectileIndex];
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        if (projectile != null && player != null)
        {
            Vector2 direction = (player.position - projectile.transform.position).normalized;
            Rigidbody2D rigidbody2D = projectile.GetComponent<Rigidbody2D>();

            if (rigidbody2D != null)
            {
                rigidbody2D.velocity = direction * projectileSpeed;
                Debug.Log("Projectile fired! Direction: " + direction);
            }
            else
            {
                Debug.LogError("Rigidbody2D not found on projectile.");
            }
        }

        currentProjectileIndex = (currentProjectileIndex + 1) % projectiles.Count;
    }

    private void UpdateHealthSprite()
    {
        if (currentHealth > maxHealth / 2)
        {
            heartSpriteRenderer.sprite = fullHealth;
        }
        else if (currentHealth > 0)
        {
            heartSpriteRenderer.sprite = halfHealth;
        }
        else
        {
            heartSpriteRenderer.sprite = noHealth;
        }
    }

    private IEnumerator Death()
    {
        isSequencePlayer = true;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        //slow motion
        Time.timeScale = slowMotion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //zoom in
        float originalSize = mainCamera.orthographicSize;
        Vector3 originalCameraPosition = mainCamera.transform.position;

        // Focus the camera on the boss
        Vector3 bossPosition = transform.position;
        mainCamera.transform.position = new Vector3(bossPosition.x, bossPosition.y, originalCameraPosition.z);
        mainCamera.orthographicSize = zoomInSize;

        GameObject explosion = null;
        if (explosionEffectPrefab != null)
        {
            explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            explosion.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(explosionDuration);
        explosion.SetActive(false);

        Collider2D bossCollider = gameObject.GetComponent<Collider2D>();
        if (bossCollider != null)
        {
            bossCollider.enabled = false;
        }

        //normal speed
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;

        GameObject spawnedChest = null;
        if (chestPrefab != null && chestSpawnPoint != null)
        {
            spawnedChest = Instantiate(chestPrefab, chestSpawnPoint.position, Quaternion.identity);
        }

        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        yield return new WaitForSeconds(2f);

        // Player jump and sit animation
        Transform playerTransform = playerMovement.transform;
        if (spawnedChest != null)
        {
            Animator playerAnimator = playerTransform.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                // Play jump animation
                playerAnimator.SetTrigger("Jumping");

                // Wait for jump animation to finish
                yield return new WaitForSeconds(0.5f);

                // Move player to the coin pile
                Vector3 coinPilePosition = spawnedChest.transform.position + new Vector3(0, 1f, 0); // Adjust offset as needed
                playerTransform.position = coinPilePosition;

                CameraFollowPlayer cameraFollowPlayer = mainCamera.GetComponent<CameraFollowPlayer>();
                if(cameraFollowPlayer != null)
                {
                    cameraFollowPlayer.enabled = false;
                }

                // Center the camera on the player
                mainCamera.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, originalCameraPosition.z);
            }
        }

        // Wait for sit animation to finish
        yield return new WaitForSeconds(1f);

        winCanvas.SetActive(true);

        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        /*CameraFollowPlayer cameraFollowEnabled = mainCamera.GetComponent<CameraFollowPlayer>();
        if (cameraFollowEnabled != null)
        {
            cameraFollowEnabled.enabled = true;
        }*/

        mainCamera.transform.position = originalCameraPosition;
        mainCamera.orthographicSize = originalSize;

        //trigger you win text here

        Destroy(gameObject);
    }
}