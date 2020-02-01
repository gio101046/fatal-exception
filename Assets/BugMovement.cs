using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public LayerMask layerMask;
    public float speed;
    private bool movingRight = true;
    Rigidbody2D bugBody;
    private Transform groundDetection;

    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void FixedUpdate()
    {


    }
}
