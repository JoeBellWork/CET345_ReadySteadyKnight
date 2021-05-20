using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightBehaviour : MonoBehaviour
{
    private Camera cam;
    Ray ray;
    RaycastHit hit;
    Vector3 trackPosition, smooth;

    public float radius, softness, smoothSpeed, scale;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        trackPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        ray = cam.ScreenPointToRay(trackPosition);

        if(Physics.Raycast(ray, out hit))
        {
            smooth = Vector3.MoveTowards(smooth, hit.point, smoothSpeed * Time.deltaTime);
            Vector4 pos = new Vector4(smooth.x, smooth.y, smooth.z, 0);
            Shader.SetGlobalVector("GlobalSpotlight_Position", pos);
        }
        
    }
}
