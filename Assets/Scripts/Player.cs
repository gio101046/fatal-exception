using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runFasterFactor = 1.5f;

    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log(HasEncounteredEnemy());
        MovePlayer();
    }

    private void MovePlayer()
    {
        Jump();
        Run();
    }

    private void Jump()
    {
        var playerOnGround = rigidbody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Ground));

        // Jump
        if (Input.GetKey(KeyCode.Space) && playerOnGround)
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

    private bool HasEncounteredEnemy()
    {
        return rigidbody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Enemies));
    }
}
