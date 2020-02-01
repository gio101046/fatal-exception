using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runFasterFactor = 1.5f;
    [SerializeField] private float groundErrorThreshold = 0.01f;

    private Rigidbody2D rigidBody;
    private BoxCollider2D collider;
    private Animator animator;
    private bool isInEncounter = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //isInEncounter = HasEncounteredEnemy();
        //if (isInEncounter)
        MovePlayer();
    }

    private void MovePlayer()
    {
        Jump();
        Run();
        FlipSprite();
        MarkAsRunning();
    }

    private void Jump()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerOnGround())
        {
            rigidBody.velocity += new Vector2(0, jumpSpeed);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x,
                                             Mathf.Clamp(rigidBody.velocity.y, 0, jumpSpeed));
        }
    }

    private void Run()
    {
        var actualRunSpeed = runSpeed;

        // Speed Run Increase
        if (Input.GetKey(KeyCode.W))
        {
            actualRunSpeed *= runFasterFactor;
        }

        // Run
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidBody.velocity = new Vector2(actualRunSpeed, rigidBody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidBody.velocity = new Vector2(actualRunSpeed * -1, rigidBody.velocity.y);
        }
    }

    private bool IsPlayerOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
                                             -Vector2.up, 
                                             1f + groundErrorThreshold, 
                                             LayerMask.GetMask(LayerNames.Ground));

        return hit.collider != null && hit.collider.IsTouching(collider);
    }

    private bool HasEncounteredEnemy()
    {
        return rigidBody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Enemies));
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }
    }

    private void MarkAsRunning()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon)
        {
            //animator.SetBool("IsRunning", true);
        }
        else
        {
            //animator.SetBool("IsRunning", false);
        }
    }
}
