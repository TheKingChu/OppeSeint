using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    public GameObject levelToActivate;
    public GameObject levelCanvas;
    public CameraFollowPlayer cameraFollowPlayer; // Reference to CameraFollowPlayer
    public Transform player;
    public Transform boss;  // Reference to the boss


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(levelToActivate != null)
            {
                FreezeLevel freezeLevel = levelToActivate.GetComponent<FreezeLevel>();
                if (freezeLevel != null)
                {
                    freezeLevel.ActivateLevel();
                }
                else
                {
                    levelToActivate.SetActive(true);
                }

                if(levelCanvas != null)
                {
                    levelCanvas.SetActive(true);
                }


                // Now that the level is activated, adjust the camera
                if (cameraFollowPlayer != null)
                {
                    cameraFollowPlayer.player = player;  // Set the player reference for the camera
                    // Check if the boss is assigned, only then set the boss reference for the camera
                    if (boss != null)
                    {
                        cameraFollowPlayer.boss = boss;  // Set the boss reference for the camera
                    }
                }

                gameObject.SetActive(false);
            }
        }
    }
}
