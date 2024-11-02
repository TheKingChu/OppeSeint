using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float pixelsPerUnit = 16f;
    public Vector3 offset = new Vector3(0, 2, -10);
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if(player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(desiredPosition);
            screenPosition.x = Mathf.Round(screenPosition.x);
            screenPosition.y = Mathf.Round(screenPosition.y);

            Vector3 roundedPosition = Camera.main.ScreenToWorldPoint(screenPosition);
            roundedPosition.z = desiredPosition.z;

            transform.position = roundedPosition;
        }
    }
}
