using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 8; //4 hearts each with 2 hits
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    private DamageEffect damageEffect;

    // Start is called before the first frame update
    void Start()
    {
        damageEffect = GetComponentInChildren<DamageEffect>();
        currentHealth = maxHealth;
        UpdateHearts();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageEffect.ShowDamageEffect();

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            GameOver();
        }
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if(currentHealth >= (i+1) * 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if(currentHealth == (i*2) + 1)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }

    private void GameOver()
    {
        Debug.Log("game over");

        //TODO add game over panel
    }
}
