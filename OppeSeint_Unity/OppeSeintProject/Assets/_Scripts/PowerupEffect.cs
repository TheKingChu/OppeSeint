using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupEffect : MonoBehaviour
{
    public GameObject powerupEffect;
    public float effectDuration = 0.5f;
    public string sortingLayerName = "PS";
    public int sortingOrder = 10;

    // Start is called before the first frame update
    void Start()
    {
        if(powerupEffect == null)
        {
            Debug.LogError("Prefab no assigned");
        }
    }

    public void PlayPowerupEffect(Vector2 pos, Transform playerTransform)
    {
        Vector3 effectPos = new Vector3(pos.x, pos.y, -1);
        GameObject effect = Instantiate(powerupEffect, effectPos, Quaternion.identity);
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
