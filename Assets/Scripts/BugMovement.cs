using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugMovement : MonoBehaviour
{
    [SerializeField] private float secondsGoingLeft = 1;
    [SerializeField] private float secondsGoingRight = 1;
    [SerializeField] private float movementSpeed = 10;
    [SerializeField] public int eventTriggerCount = 3;
    [SerializeField] public int controlTriggerCounterEvent = 3;
    [SerializeField] Collider2D playerCollider;
    [SerializeField] EventControls eventControls;
    [SerializeField] Player player;

    private int leftAccumalator = 0;
    private int rightAccumalator = 0;
    private Rigidbody2D rigidBody;
    private bool isMovementDisabled = false;

    private int framesPerSecond => 60;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (HasEncounteredPlayer())
            eventControls.TriggerEvent(playerCollider, GetComponent<Collider2D>(), player);
        if (isMovementDisabled)
            return;

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
        rigidBody.velocity = new Vector2(actualSpeed, rigidBody.velocity.y);
    }

    private void MoveRight()
    {
        var actualSpeed = movementSpeed;
        rigidBody.velocity = new Vector2(actualSpeed, rigidBody.velocity.y);
    }

    private bool HasEncounteredPlayer()
    {
        return rigidBody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Player));
    }

    public void DisableMovement()
    {
        isMovementDisabled = true;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void EnableMovement()
    {
        isMovementDisabled = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
    }
}
