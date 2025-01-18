using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingEffect : MonoBehaviour
{
    public GameObject healingEffect;
    public float effectDuration = 0.5f;
    public string sortingLayerName = "PS";
    public int sortingOrder = 10;

    // Start is called before the first frame update
    void Start()
    {
        if(healingEffect == null)
        {
            Debug.LogError("Prefab not assigned");
        }
    }

    public void PlayHealingEffect(Vector2 pos, Transform playerTransform)
    {
        Vector3 effectPos = new Vector3(pos.x, pos.y, -1);
        GameObject effect = Instantiate(healingEffect, effectPos, Quaternion.identity);
        effect.transform.SetParent(playerTransform);

        ParticleSystemRenderer particleSystemRenderer = effect.GetComponent<ParticleSystemRenderer>();
        if (particleSystemRenderer != null)
        {
            particleSystemRenderer.sortingLayerName = sortingLayerName;
            particleSystemRenderer.sortingOrder = sortingOrder;
        }
        Destroy(effect, effectDuration);
    }
}
