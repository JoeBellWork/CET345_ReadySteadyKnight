using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + .1f, transform.position.y);
    }
}
