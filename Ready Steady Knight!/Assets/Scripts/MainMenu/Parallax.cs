using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject camObj;
    public float parallaxEffect;

    private float mapLength, startPos, distance, temp;



    void Start()
    {
        startPos = transform.position.x;
        mapLength = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // continously moves the parralax background with using the parallaxEffect float amount. This makes different layers move faster and slower than others.
    //if the background leaves frame, moves to the other side of the grouping so it creates and endlss loop.
    private void FixedUpdate()
    {
        temp = camObj.transform.position.x * (1 - parallaxEffect);
        distance = camObj.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + mapLength)
        {
            startPos += mapLength;
        }
        else if (temp < startPos - mapLength)
        {
            startPos -= mapLength;
        }
    }
}
