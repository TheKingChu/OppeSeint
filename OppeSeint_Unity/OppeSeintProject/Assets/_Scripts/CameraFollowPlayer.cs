using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public Transform boss;  // Reference to the boss
    public float pixelsPerUnit = 16f;
    public Vector3 offsetLeft = new Vector3(5, 2, -10);  // Offset for the left side of the boss
    public Vector3 offsetRight = new Vector3(-5, 2, -10);  // Offset for the right side of the boss
    public float smoothSpeed = 0.125f;

    private Vector3 currentOffset;

    private void Start()
    {
        // Initialize the current offset based on player's initial position
        if (player.position.x < boss.position.x)
        {
            currentOffset = offsetLeft;
        }
        else
        {
            currentOffset = offsetRight;
        }
    }

    private void LateUpdate()
    {
        if (player != null && boss != null)
        {
            // Check if the player is on the left or right side of the boss
            Vector3 desiredPosition = player.position + currentOffset;

            // Smoothly transition to the new offset
            currentOffset = Vector3.Lerp(currentOffset, GetCameraOffset(), smoothSpeed);

            // Calculate and set the camera position
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(desiredPosition);
            screenPosition.x = Mathf.Round(screenPosition.x);
            screenPosition.y = Mathf.Round(screenPosition.y);

            Vector3 roundedPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            roundedPosition.z = desiredPosition.z;

            transform.position = roundedPosition;
        }
    }

    private Vector3 GetCameraOffset()
    {
        // Compare player’s position to the boss’s position to determine left or right
        if (player.position.x < boss.position.x)
        {
            // Player is on the left side of the boss
            return offsetLeft;
        }
        else
        {
            // Player is on the right side of the boss
            return offsetRight;
        }
    }
}
