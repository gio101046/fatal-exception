using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runFasterFactor = 1.5f;
    [SerializeField] private float groundErrorThreshold = 0.01f;

    private Rigidbody2D rigidbody;
    private PolygonCollider2D collider;
    private bool isInEncounter = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<PolygonCollider2D>();
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
    }

    private void Jump()
    {
        // Jump
        if (Input.GetKey(KeyCode.Space) && IsPlayerOnGround())
        {
            rigidbody.velocity += new Vector2(0, jumpSpeed);
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,
                                             Mathf.Clamp(rigidbody.velocity.y, 0, jumpSpeed));
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
            rigidbody.velocity = new Vector2(actualRunSpeed, rigidbody.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.velocity = new Vector2(actualRunSpeed * -1, rigidbody.velocity.y);
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
        return rigidbody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Enemies));
    }
}
