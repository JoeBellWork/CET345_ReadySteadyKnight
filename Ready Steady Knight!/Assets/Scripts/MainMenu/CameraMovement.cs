using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // attack to main mnu camera for continuous movement along the X axis. Usd for parralax Scrolling.
    private void FixedUpdate()
    {
        transform.position = new Vector2(transform.position.x + .1f, transform.position.y);
    }
}
