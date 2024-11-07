using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossScript : MonoBehaviour
{
    [SerializeField] private int health = 10;
    [SerializeField] private float speed = 1f;
    public Transform pointA;
    public Transform pointB;
    private Transform targetPoint;

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
        Debug.Log($"Distance to target ({targetPoint.name}): {distanceToTarget}");

        // Use a slightly larger threshold to ensure the switch
        if (distanceToTarget < 0.2f)  // Try a slightly larger threshold here
        {
            // Switch target point
            targetPoint = targetPoint == pointA ? pointB : pointA;
            Debug.Log($"Switching target to: {targetPoint.name}");
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
        health -= damage;
        if(health <= 0)
        {
            StartCoroutine(Death());
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
