using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeLevel : MonoBehaviour
{
    public bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        if (!isActive)
        {
            foreach(var component in GetComponentsInChildren<MonoBehaviour>())
            {
                component.enabled = false;
            }
        }
    }

    public void ActivateLevel()
    {
        isActive = true;
        foreach (var componetn in GetComponentsInChildren<MonoBehaviour>())
        {
            componetn.enabled = true;
        }
    }
}
