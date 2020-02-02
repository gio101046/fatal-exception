using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float runFasterFactor = 1.5f;
    [SerializeField] private float groundErrorThreshold = 0.05f;
    [SerializeField] private float wallLineCaseDistance = 0.5f;
    [SerializeField] private float runErrorThreshold = 0.05f;
    [SerializeField] private int startHealth = 3;
    private int currentHealth;
    [SerializeField] private int startStamina = 100;
    [SerializeField] private int coffeValuePercent = 10;
    [SerializeField] private int bugStaminaDamagePercent = 15;
    private int currentStamina;

    [SerializeField] private SpriteRenderer healthBar;
    [SerializeField] private SpriteRenderer staminaBar;


    private Rigidbody2D rigidBody;
    new private BoxCollider2D collider;
    private Animator animator;

    private Vector2 wallLineCaseDistanceVector => new Vector2(wallLineCaseDistance, 0);

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        currentHealth = startHealth;
        currentStamina = startStamina;
        healthBar.size = new Vector2(1.5f * startHealth, healthBar.size.y);
        staminaBar.size = new Vector2(staminaBar.size.x, staminaBar.size.y);
    }

    private void Update()
    { 
        MovePlayer();
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Pizza")
        {
            Destroy(coll.gameObject);
            currentHealth += currentHealth < startHealth ? 1 : 0;
            healthBar.size = new Vector2(1.5f * currentHealth, this.healthBar.size.y);
        }
        else if (coll.gameObject.tag == "Coffee")
        {
            Destroy(coll.gameObject);
            DrinkCoffee();
        }
    }

    private void DrinkCoffee()
    {
        int result = currentStamina + GetStaminaValueChange(coffeValuePercent);
        currentStamina = result < startStamina ? result : startStamina;
        staminaBar.size = new Vector2(staminaBar.size.x * (currentStamina/startStamina), staminaBar.size.y);
    }

    private void DecreaseStaminaAfterBugFight()
    {
        int result = currentStamina - GetStaminaValueChange(bugStaminaDamagePercent);
        currentStamina = result < 0 ? 0 : result;
    }

    private int GetStaminaValueChange(int percentage)
    {
        return currentStamina < startStamina ? (startStamina * percentage / 100) : 0;
    }


    private void MovePlayer()
    {
        Jump();
        Run();
        FlipSprite();
        HandleAnimations();
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
        if (IsPlayerOnWall())
            return;

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
        return IsPointOnGround(transform.position + new Vector3(0.3f, 0, 0)) ||
               IsPointOnGround(transform.position + new Vector3(-0.3f, 0, 0));
    }

    private bool IsPointOnGround(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position,
                                             -Vector2.up,
                                             groundErrorThreshold,
                                             LayerMask.GetMask(LayerNames.Ground));

        return hit.collider != null && hit.collider.IsTouching(collider);
    }

    private bool IsPlayerOnWall()
    {
        return IsPointOnWall(transform.position + new Vector3(0, 1, 0)) ||
               IsPointOnWall(transform.position + new Vector3(0, -1, 0)) ||
               IsPointOnWall(transform.position + new Vector3(0, -2, 0));
    }

    private bool IsPointOnWall(Vector2 position)
    {
        var rightHit = Physics2D.Linecast(position,
                                          position + wallLineCaseDistanceVector,
                                          LayerMask.GetMask(LayerNames.Ground));
        var leftHit = Physics2D.Linecast(position,
                                         position - wallLineCaseDistanceVector,
                                         LayerMask.GetMask(LayerNames.Ground));

        return ((rightHit.collider != null && rightHit.collider.IsTouching(collider)) ||
               (leftHit.collider != null && leftHit.collider.IsTouching(collider))) && !IsPlayerOnGround();
    }

    private bool HasEncounteredEnemy()
    {
        return rigidBody.IsTouchingLayers(LayerMask.GetMask(LayerNames.Enemies));
    }

    private void FlipSprite()
    {
        if (Mathf.Abs(rigidBody.velocity.x) > runErrorThreshold && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }
    }

    private void HandleAnimations()
    {
        animator.SetBool("IsRunning", Mathf.Abs(rigidBody.velocity.x) > runErrorThreshold);
        animator.SetBool("IsGround", IsPlayerOnGround());
        animator.SetFloat("YVelocity", rigidBody.velocity.y);
    }
}
