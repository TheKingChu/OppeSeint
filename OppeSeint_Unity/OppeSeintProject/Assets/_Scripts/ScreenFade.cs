using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetFade(0f);
    }

    public IEnumerator FadeIn()
    {
        yield return Fade(1f);
    }

    public IEnumerator FadeOut()
    {
        yield return Fade(0f);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            SetFade(newAlpha);
            yield return null;
        }

        SetFade(targetAlpha);
    }

    private void SetFade(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
