using UnityEngine;

public class SpotLightBehaviour : MonoBehaviour
{
    // script used in shader practise to develop spotlight controls
    // camera, ray, hit used to creat raycast for spotlight to follow mouse position, track positon is mouse position and smooth is the smoothing speed to improve movement.
    private Camera cam;
    Ray ray;
    RaycastHit hit;
    Vector3 trackPosition, smooth;

    public float radius, softness, smoothSpeed, scale;
    // public control varibles of spotlight size, blur and speed

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            radius += scale * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            radius -= scale * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            softness -= scale * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            softness += scale * Time.deltaTime;
        }

        Mathf.Clamp(radius, 0, 100);
        Mathf.Clamp(softness, 0, 100);

        trackPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        ray = cam.ScreenPointToRay(trackPosition);

        if (Physics.Raycast(ray, out hit))
        {
            smooth = Vector3.MoveTowards(smooth, hit.point, smoothSpeed * Time.deltaTime);
            Vector4 pos = new Vector4(smooth.x, smooth.y, smooth.z, 0);
            Shader.SetGlobalVector("GlobalSpotlight_Position", pos);
        }
        Shader.SetGlobalFloat("GlobalSpotlight_Radius", radius);
        Shader.SetGlobalFloat("GlobalSpotlight_Softness", softness);
    }
}
