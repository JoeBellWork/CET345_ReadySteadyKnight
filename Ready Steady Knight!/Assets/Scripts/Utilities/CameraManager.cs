using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // varibles for camera and position
    public Transform cameraHolder;
    public List<Transform> players = new List<Transform>();
    Vector3 middlePoint;

    // orthographic camera limits
    public float orthoMin = 2;
    public float orthoMax = 6;

    // perspective camera target and limits
    float targetZ;
    public float zMin = 5;
    public float zMax = 10;

    Camera cam;
    public CameraType cType;

    [Space(30)]
    // spotlight shader varibles
    public Transform target;
    Vector3 trackPosition, smooth;

    public float radius, softness, smoothSpeed, scale;

    public enum CameraType
    {
        ortho,
        perspective
    }

    void Start()
    {
        // access camera and then determin what style of camera is being used.
        cam = Camera.main;
        cameraHolder = cam.transform.parent;
        cType = (cam.orthographic) ? CameraType.ortho : CameraType.perspective;

        // shader varibles for bluring of spotlight
        Shader.SetGlobalFloat("GlobalSpotlight_Radius", radius);
        Shader.SetGlobalFloat("GlobalSpotlight_Softness", softness);
    }

    void FixedUpdate()
    {
        // track midpoint between 2 players for spotlight position and camera position
        float distance = Vector3.Distance(players[0].position, players[1].position);
        float half = (distance / 2);

        middlePoint = (players[1].position - players[0].position).normalized * half;
        middlePoint += players[0].position;

        switch (cType)
        {
            case CameraType.ortho:
                cam.orthographicSize = 2 * (half / 2);
                if (cam.orthographicSize > orthoMax)
                {
                    cam.orthographicSize = orthoMax;
                }

                if (cam.orthographicSize < orthoMin)
                {
                    cam.orthographicSize = orthoMin;
                }
                break;

            case CameraType.perspective:
                targetZ = -(2 * (half / 2));
                if (Mathf.Abs(targetZ) < Mathf.Abs(zMin))
                {
                    targetZ = zMin;
                }
                if (Mathf.Abs(targetZ) > Mathf.Abs(zMax))
                {
                    targetZ = zMax;
                }

                cam.transform.localPosition = new Vector3(0, 0.5f, targetZ);
                break;
        }
        cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, middlePoint, Time.deltaTime * 5);

        // add shader controls to camera manager of levels to allow spotlight to appear on  tilemap.
        trackPosition = new Vector3(target.position.x, target.position.y, target.position.z);
        smooth = Vector3.MoveTowards(smooth, trackPosition, smoothSpeed * Time.deltaTime);
        Vector4 pos = new Vector4(smooth.x, smooth.y, 0, 0);
        Shader.SetGlobalVector("GlobalSpotlight_Position", pos);
    }


    // static control
    public static CameraManager instance;
    public static CameraManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
}
