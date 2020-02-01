using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float offsetY = 0;
    [SerializeField] float interpolate = 0.2f; 

    private void Start()
    {
        
    }

    private void Update()
    {
        transform.position = Vector2.Lerp(transform.position, player.transform.position + new Vector3(0, offsetY, 0), interpolate);
    }
}
