using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    public Image damageOverlay;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] float maxAlpha = 0.5f;

    private Color overlayColor;
    private bool isFlashing;

    // Start is called before the first frame update
    void Start()
    {
        if(damageOverlay != null)
        {
            overlayColor = damageOverlay.color;
            overlayColor.a = 0;
            damageOverlay.color = overlayColor;
            damageOverlay.enabled = true;
        }
    }

    public void ShowDamageEffect()
    {
        if (!isFlashing)
        {
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        isFlashing = true;

        float timer = 0;
        while(timer < flashDuration)
        {
            timer += Time.deltaTime;
            overlayColor.a = Mathf.Lerp(0, maxAlpha, timer / flashDuration);
            damageOverlay.color = overlayColor;
            yield return null;
        }

        timer = 0;
        while( timer < flashDuration)
        {
            timer += Time.deltaTime;
            overlayColor.a = Mathf.Lerp(maxAlpha, 0, timer / flashDuration);
            damageOverlay.color = overlayColor;
            yield return null;
        }

        isFlashing = false;
    }
}
