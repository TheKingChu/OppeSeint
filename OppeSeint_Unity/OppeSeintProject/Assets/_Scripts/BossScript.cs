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
    private Coroutine hitBlinkCoroutine;

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
    public GameObject coinParticleEffectPrefab;

    [Header("Projectile")]
    public Transform projectileContainer;
    public List<BossProjectile> projectiles = new List<BossProjectile>();
    [SerializeField] private float projectileSpeed = 2f;
    private int currentProjectileIndex = 0;
    private Dictionary<BossProjectile, Vector3> initialProjectilePos = new Dictionary<BossProjectile, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        ResetBoss();

        foreach (BossProjectile projectile in projectiles)
        {
            projectile.bossScript = this;
        }
    }

    private void OnEnable()
    {
        ResetBoss();
    }

    private void ResetBoss()
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
                    projectile.bossScript = this;

                    //Store the initial position of the projectile
                    if (!initialProjectilePos.ContainsKey(projectile))
                    {
                        initialProjectilePos[projectile] = projectile.transform.position;
                    }
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

        if (hitBlinkCoroutine != null)
        {
            StopCoroutine(hitBlinkCoroutine);
        }
        hitBlinkCoroutine = StartCoroutine(BlinkEffect());

        ShootProjectile();

        if (currentHealth <= 0 && !isSequencePlayer)
        {
            StartCoroutine(Death());
        }
    }

    private IEnumerator BlinkEffect()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if(spriteRenderer == null)
        {
            yield break; // Exit if sprite renderer is not found
        }

        //blink
        for(int i = 0; i < 1; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        spriteRenderer.color = Color.white;
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

    public void CheckAllProjectilesDestroyed()
    {
        foreach(BossProjectile projectile in projectiles)
        {
            if (projectile.gameObject.activeSelf)
            {
                return; //if atleast one projectile is active, return
            }
        }

        StartCoroutine(ResetAllProjectiles());
    }

    private IEnumerator ResetAllProjectiles()
    {
        yield return new WaitForSeconds(3f);
        foreach (BossProjectile projectile in projectiles)
        {
            if(initialProjectilePos.ContainsKey(projectile))
            {
                projectile.transform.position = initialProjectilePos[projectile];
            }
            projectile.gameObject.SetActive(true);
        }

        Debug.Log("Projectiles reset!");
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

        //destoy all projectiles
        foreach (BossProjectile projectile in projectiles)
        {
            if(projectile != null)
            {
                Destroy(projectile.gameObject);
            }
        }
        projectiles.Clear();

        // Slow motion
        Time.timeScale = slowMotion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        // Zoom in
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

        // Normal speed
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
                if (cameraFollowPlayer != null)
                {
                    cameraFollowPlayer.enabled = false;
                }

                // Center the camera on the player
                mainCamera.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, originalCameraPosition.z);
            }
        }

        // Wait for sit animation to finish
        yield return new WaitForSeconds(0.4f);

        // Teleport player and chest to the specified position
        Vector3 teleportPosition = new Vector3(148f, -4.4f, 0f);
        playerTransform.position = teleportPosition;
        if (spawnedChest != null)
        {
            spawnedChest.transform.position = teleportPosition;

            // Instantiate coin particle effect
            Vector3 effectPos = new Vector3(spawnedChest.transform.position.x, spawnedChest.transform.position.y, -1);
            GameObject coinParticleEffect = Instantiate(coinParticleEffectPrefab, effectPos, Quaternion.identity);
            coinParticleEffect.SetActive(true);
        }

        // Center the camera on the new position
        mainCamera.transform.position = new Vector3(teleportPosition.x, -3.11f, originalCameraPosition.z);
        mainCamera.orthographicSize = 6.11f;

        winCanvas.SetActive(true);

        // Re-enable player movement
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        mainCamera.orthographicSize = originalSize;

        // Trigger you win text here

        Destroy(gameObject);
    }
}