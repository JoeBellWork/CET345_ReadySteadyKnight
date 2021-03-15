using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject camObj;
    public float parallaxEffect;

    private float mapLength, startPos, distance, temp;


    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        mapLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void FixedUpdate()
    {
        temp = camObj.transform.position.x * (1 - parallaxEffect);
        distance = camObj.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if(temp > startPos + mapLength)
        {
            startPos += mapLength;
        }
        else if(temp < startPos - mapLength)
        {
            startPos -= mapLength;
        }
    }
}
