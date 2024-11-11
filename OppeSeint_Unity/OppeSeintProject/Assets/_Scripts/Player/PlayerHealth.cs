using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 8; //4 hearts each with 2 hits
    public int currentHealth;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public GameObject loseCanvas;

    private DamageEffect damageEffect;

    // Start is called before the first frame update
    void Start()
    {
        damageEffect = GetComponentInChildren<DamageEffect>();
        loseCanvas.SetActive(false);

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

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        UpdateHearts();
    }

    private void GameOver()
    {
        //TODO add game over panel
        Time.timeScale = 0f;
        loseCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
