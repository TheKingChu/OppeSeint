using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenTextEffect : MonoBehaviour
{
    public TMP_Text winCoinText;
    public float incrementSpeed = 1000f;
    public float rainbowSpeed = 1f;

    private int targetCoinCount = 100000;
    private int currentCoinCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IncrementCoinCount());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRainbowEffect();
    }

    private IEnumerator IncrementCoinCount()
    {
        while (currentCoinCount < targetCoinCount)
        {
            currentCoinCount += (int)(incrementSpeed * Time.deltaTime);
            if (currentCoinCount > targetCoinCount)
            {
                currentCoinCount = targetCoinCount;
            }
            winCoinText.text = currentCoinCount.ToString();
            yield return null;
        }
    }

    private void ApplyRainbowEffect()
    {
        float hue = Time.time * rainbowSpeed;
        hue %= 1;
        winCoinText.color = Color.HSVToRGB(hue, 1, 1);
    }
}
