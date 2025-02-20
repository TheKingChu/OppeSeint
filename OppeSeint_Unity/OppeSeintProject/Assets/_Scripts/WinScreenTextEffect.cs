using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WinScreenTextEffect : MonoBehaviour
{
    public TMP_Text winCoinText;
    public float incrementSpeed = 10000f;
    public float rainbowSpeed = 1f;
    public float waveFrequency = 2f;
    public float waveAmplitude = 5f;

    private int targetCoinCount = 100000;
    private int currentCoinCount = 0;
    private TMP_TextInfo textInfo;

    // Start is called before the first frame update
    void Start()
    {
        textInfo = winCoinText.textInfo;
        StartCoroutine(IncrementCoinCount());
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRainbowEffect();
        ApplyWaveEffect();
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
            winCoinText.text = FormatCoinCount(currentCoinCount);
            yield return null;
        }
    }

    private void ApplyRainbowEffect()
    {
        float hue = Time.time * rainbowSpeed;
        hue %= 1;
        winCoinText.color = Color.HSVToRGB(hue, 1, 1);
    }

    private void ApplyWaveEffect()
    {
        winCoinText.ForceMeshUpdate();
        textInfo = winCoinText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
            {
                continue;
            }

            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            Vector3[] vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

            float offset = Mathf.Sin(Time.time * waveFrequency + i * 0.1f) * waveAmplitude;
            vertices[vertexIndex + 0].y += offset;
            vertices[vertexIndex + 1].y += offset;
            vertices[vertexIndex + 2].y += offset;
            vertices[vertexIndex + 3].y += offset;
        }

        for(int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            winCoinText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }

    private string FormatCoinCount(int coinCount)
    {
        return coinCount.ToString("N0").Replace(",", " ");
    }
}
