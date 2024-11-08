using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossScript : MonoBehaviour
{
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

    [SerializeField] private int damageToPlayer = 2;

    public GameObject explosionEffectPrefab;
    public Camera mainCamera;
    [SerializeField] private float slowMotion = 0.1f;
    [SerializeField] private float zoomInSize = 3f;
    //[SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float explosionDuration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        targetPoint = pointB;

        if(mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        Patrole();
    }

    private void Patrole()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        //Check if the boss has reached the target point
        float distanceToTarget = Vector2.Distance(transform.position, targetPoint.position);

        if (distanceToTarget < 0.2f)
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
            if(playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthSprite();
        if(currentHealth <= 0)
        {
            StartCoroutine(Death());
        }
    }

    private void UpdateHealthSprite()
    {
        if (currentHealth > maxHealth / 2)
        {
            heartSpriteRenderer.sprite = fullHealth;
        }
        else if(currentHealth > 0)
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
        //slow motion
        Time.timeScale = slowMotion;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        //zoom in
        float originalSize = mainCamera.orthographicSize;
        mainCamera.orthographicSize = zoomInSize;

        if(explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }

        yield return new WaitForSecondsRealtime(explosionDuration);

        //normal speed
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.02f;
        mainCamera.orthographicSize = originalSize;

        Destroy(gameObject);
    }
}
