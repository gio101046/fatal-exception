using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    public LayerMask layerMask;
    public float speed;
    Rigidbody2D bugBody;
    Transform bugTrans;
    float bugWidth;


    // Start is called before the first frame update
    void Start()
    {
        bugTrans = this.transform;
        bugBody = this.GetComponent<Rigidbody2D>();
        bugWidth = this.GetComponent<SpriteRenderer>().bounds.extents.x;
    }

    // Update is called once per frame
    void Update()
    {
        // CHECK IF GROUNDED
        //Vector2 lineCastPos = bugTrans.position - bugTrans.right * bugWidth;

        //bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, layerMask);

        //if (!isGrounded)
        //{
        //    Vector3 currentRotation = bugTrans.eulerAngles;
        //    currentRotation.y += 180;
        //    bugTrans.eulerAngles = currentRotation;
        //}


        // Move Forward
        Vector2 bugVelocity = bugBody.velocity;
        bugVelocity.x = speed;
        bugBody.velocity = bugVelocity;
    }

    void FixedUpdate()
    {


    }
}
