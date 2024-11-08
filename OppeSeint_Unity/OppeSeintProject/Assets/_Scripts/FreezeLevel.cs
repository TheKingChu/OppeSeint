using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeLevel : MonoBehaviour
{
    public bool isActive = false;

    private void Awake()
    {
        if (!isActive)
        {
            DisableLevelComponents();
        }
    }

    public void ActivateLevel()
    {
        isActive = true;
        EnableLevelComponents();
    }

    private void DisableLevelComponents()
    {
        if (!isActive)
        {
            foreach (var component in GetComponentsInChildren<MonoBehaviour>())
            {
                component.enabled = false;
            }
        }
    }

    private void EnableLevelComponents()
    {
        foreach (var componetn in GetComponentsInChildren<MonoBehaviour>())
        {
            componetn.enabled = true;
        }
    }
}
