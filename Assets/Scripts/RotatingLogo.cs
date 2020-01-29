using UnityEngine;

public class RotatingLogo : MonoBehaviour
{
    public Vector3 speed;

    void Update()
    {
        transform.Rotate(speed);
    }
}
