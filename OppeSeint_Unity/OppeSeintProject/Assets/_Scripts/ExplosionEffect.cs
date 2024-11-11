using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public GameObject explosionEffect;

    public void PlayExplosionEffect()
    {
        if(explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
