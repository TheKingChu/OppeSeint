using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    public GameObject healingEffect;
    public float effectDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if(healingEffect == null)
        {
            Debug.LogError("Prefab not assigned");
        }
    }

    public void PlayHealingEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(healingEffect, pos, Quaternion.identity);
        Destroy(effect, effectDuration);
    }
}
