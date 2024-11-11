using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    public int maxJumps = 2;


    private Rigidbody2D rb2d;
    private int jumpCount;
    private Vector2 startTouchPosition;
    private Vector2 direction;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer gunRenderer;
    private bool isDragging = false;

    public Vector2 gunOffset = new Vector2(0.5f, 0);

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    isDragging = true; 
                    break;

                case TouchPhase.Moved:
                    Vector2 currentTouchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector2 startWorldPosition = Camera.main.ScreenToWorldPoint(startTouchPosition);
                    direction = (currentTouchPosition - startWorldPosition).normalized;
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    direction = Vector2.zero;
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDragging)
        {
            rb2d.velocity = new Vector2(direction.x * moveSpeed, rb2d.velocity.y);

            if(direction.x != 0)
            {
                spriteRenderer.flipX = direction.x < 0;
                gunRenderer.flipX = direction.x < 0;
                UpdateGunPosition();
            }
        }
        else
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    private void UpdateGunPosition()
    {
        Vector2 playerPos = transform.position;
        Vector2 gunPos = playerPos + (direction.x < 0 ? -gunOffset : gunOffset);
        gunRenderer.transform.position = new Vector3(gunPos.x, gunPos.y, gunRenderer.transform.position.z);
    }

    public void JumpButton()
    {
        if(jumpCount < maxJumps)
        {
            animator.SetTrigger("Jumping");
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpCount++;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
        }
    }
}
