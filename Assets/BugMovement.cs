using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [SerializeField] private int secondsGoingLeft = 1;
    [SerializeField] private int secondsGoingRight = 1;
    [SerializeField] private float movementSpeed = 10;

    private int leftAccumalator = 0;
    private int rightAccumalator = 0;
    private Rigidbody2D rigidBody;

    private int framesPerSecond => 60;

    private void Start()
    {
        // Physics2D.IgnoreLayerCollision(LayerMask.GetMask())

        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        var totalFramesGoingLeft = framesPerSecond * secondsGoingLeft;
        var totalFramesGoingRight = framesPerSecond * secondsGoingRight;

        if (leftAccumalator <= totalFramesGoingLeft)
        {
            MoveLeft();
            leftAccumalator++;

            if (leftAccumalator == totalFramesGoingLeft)
                rightAccumalator = 0;
        }
        else if (rightAccumalator <= totalFramesGoingRight)
        {
            MoveRight();
            rightAccumalator++;

            if (rightAccumalator == totalFramesGoingRight)
                leftAccumalator = 0;
        }
    }

    private void MoveLeft()
    {
        var actualSpeed = movementSpeed * -1;
        rigidBody.velocity += new Vector2(actualSpeed, 0);
    }

    private void MoveRight()
    {
        var actualSpeed = movementSpeed;
        rigidBody.velocity += new Vector2(actualSpeed, 0);
    }
}
