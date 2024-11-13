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
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            ParticleSystem ps = explosion.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // Wait for the particle system to finish, then destroy the explosion effect
                Destroy(explosion, ps.main.duration);
            }
            else
            {
                // If there's no ParticleSystem, just destroy the explosion effect after a fixed time
                Destroy(explosion, 1f);  // Adjust time if necessary
            }
        }
    }
}
