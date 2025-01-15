using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance { get; private set; }

    public TMP_Text coinText;
    public int coinCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin()
    {
        coinCount++;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "" + coinCount;
    }
}
