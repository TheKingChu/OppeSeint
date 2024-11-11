using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxClouds : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public float cloudWidth = 25f;
    public bool loop = true;

    private float startPosX;
    private float newPosX;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        newPosX = Mathf.Repeat(Time.time * moveSpeed, cloudWidth);
        transform.position = new Vector3(startPosX + newPosX, transform.position.y, transform.position.z);
    }
}
