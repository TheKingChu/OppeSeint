using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffect : MonoBehaviour
{
    public GameObject powerupEffect;
    public float effectDuration = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        if(powerupEffect == null)
        {
            Debug.LogError("Prefab no assigned");
        }
    }

    public void PlayPowerupEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(powerupEffect, pos, Quaternion.identity);
        Destroy(effect, effectDuration);
    }
}
