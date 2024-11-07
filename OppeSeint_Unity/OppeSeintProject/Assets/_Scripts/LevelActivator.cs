using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelActivator : MonoBehaviour
{
    public GameObject levelToActivate;


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
                gameObject.SetActive(false);
            }
        }
    }
}
