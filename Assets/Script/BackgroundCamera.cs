using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCamera : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxEffectMultiplier;

    private float xPosition;
    private float length;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1-parallaxEffectMultiplier);
        float distance = cam.transform.position.x * parallaxEffectMultiplier;

        transform.position = new Vector3(xPosition + distance, transform.position.y);
        if (distanceMoved > xPosition + length)
        {
            xPosition += length;
        }
        else if (distanceMoved < xPosition - length)
        {
            xPosition -= length;
        }
    }
}
