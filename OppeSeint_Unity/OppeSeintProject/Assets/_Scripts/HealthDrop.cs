using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDrop : MonoBehaviour
{
    public int healtAmount = 1;
    private HealingEffect healingEffect;

    private void Start()
    {
        healingEffect = GetComponent<HealingEffect>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if(playerHealth != null)
            {
                playerHealth.Heal(healtAmount);
                if(healingEffect != null)
                {
                    healingEffect.PlayHealingEffect(collision.transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}
