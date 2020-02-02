using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpSpeed = 35f;
    [SerializeField] private float runFasterFactor = 1.5f;
    [SerializeField] private float groundErrorThreshold = 1.5f;
    [SerializeField] private float wallErrorThreshold = 1.5f;
    [SerializeField] private float wallLineCaseDistance = 0.5f;
    [SerializeField] private float runErrorThreshold = 0.05f;
    [SerializeField] private int startHealth = 3;
    [SerializeField] private int startStamina = 100;
    [SerializeField] private int coffeValuePercent = 18;
    [SerializeField] private int bugStaminaDamagePercent = 15;
    [SerializeField] private float hurtVelocity = 15f;
    [SerializeField] private int fightSoundLength = 5;

    private int currentHealth;
    private int currentStamina;
    private float staminaInitialSize;

    [SerializeField] private SpriteRenderer healthBar;
    [SerializeField] private SpriteRenderer staminaBar;

    private Rigidbody2D rigidBody;
    new private Collider2D collider;
    private Animator animator;
    private bool isMovementEnabled = true;
    private bool isFighting = false;
    private bool isHurt = false;
    private int fightSoundAccumalator = 0;
    private bool isPlayerHurt = false;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        currentHealth = startHealth;
        healthBar.size = new Vector2(1.5f * startHealth, healthBar.size.y);

        currentStamina = startStamina;
        staminaInitialSize = staminaBar.size.x;
        staminaBar.size = new Vector2(staminaInitialSize, staminaBar.size.y);
    }

    private void Update()
    { 
        MovePlayer();

        //if (IsPlayerOnWall(true))
        //{
        //    if (collider.sharedMaterial == null || collider.sharedMaterial.friction != 0f)
        //    {
        //        collider.sharedMaterial = new PhysicsMaterial2D();
        //        collider.sharedMaterial.friction = 0f;
        //        collider.enabled = false;
        //        collider.enabled = true;
        //    }
        //}
        //else
        //{
        //    if (collider.sharedMaterial == null || collider.sharedMaterial.friction == 0f)
        //    {
        //        collider.sharedMaterial = new PhysicsMaterial2D();
        //        collider.sharedMaterial.friction = 0.5f;
        //        collider.enabled = false;
        //        collider.enabled = true;
        //    }
        //}

    }

    public void OnConsumable(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pizza")
        {
            SoundManagerScript.PlaySound("eat");
            Destroy(collision.gameObject);
            currentHealth += currentHealth < startHealth ? 1 : 0;
            healthBar.size = new Vector2(1.5f * currentHealth, this.healthBar.size.y);
        }
        else if (collision.gameObject.tag == "Coffee")
        {
            Destroy(collision.gameObject);
            DrinkCoffee();
        }
    }

    private void DrinkCoffee()
    {
        SoundManagerScript.PlaySound("drink");
        int result = currentStamina + GetStaminaValueChange(coffeValuePercent);
        currentStamina = result < startStamina ? result : startStamina;
        float multiplier = currentStamina * 1f / startStamina;
        staminaBar.size = new Vector2(staminaInitialSize * multiplier, staminaBar.size.y);
    }

    private void DecreaseStaminaAfterBugFight()
    {
        int result = currentStamina - GetStaminaValueChange(bugStaminaDamagePercent);
        currentStamina = result < 0 ? 0 : result;
        float multiplier = currentStamina * 1f / startStamina;
        staminaBar.size = new Vector2(staminaInitialSize * multiplier, staminaBar.size.y);
    }

    private int GetStaminaValueChange(int percentage)
    {
        return currentStamina <= startStamina ? (startStamina * percentage / 100) : 0;
    }

    private void Hurt()
    {
        currentHealth -= currentHealth > 0 ? 1 : 0;
        healthBar.size = new Vector2(1.5f * currentHealth, this.healthBar.size.y);

        if (currentHealth <= 0) GameOver();
    }

    private void GameOver() 
    {
        SceneManager.LoadScene("GameOver");
    }


    private void MovePlayer()
    {
        if (isMovementEnabled)
        {
            Jump();
            Run();
            FlipSprite();
        }

        if (Mathf.Abs(rigidBody.velocity.y) < runErrorThreshold)
        {
            isHurt = false;
        };

        PlayFightSound();
        HandleAnimations();
    }

    private void Jump()
    {
        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsPlayerOnGround())
        {
            SoundManagerScript.PlaySound("jump");
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
        if (IsPlayerOnWall())
        {
            actualRunSpeed = 0;
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

        // Run
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    private bool IsPlayerOnGround()
    {
        return IsPointOnGround(transform.position + new Vector3(collider.bounds.extents.x * 0.96f, 0, 0)) ||
               IsPointOnGround(transform.position) ||
               IsPointOnGround(transform.position - new Vector3(collider.bounds.extents.x * 0.99f, 0, 0));
    }

    private bool IsPointOnGround(Vector2 position)
    {
        Debug.DrawLine(position, position - new Vector2(0, collider.bounds.extents.y * groundErrorThreshold));

        RaycastHit2D hit = Physics2D.Raycast(position,
                                             -Vector2.up,
                                             collider.bounds.extents.y * groundErrorThreshold,
                                             LayerMask.GetMask(LayerNames.Ground));

        return hit.collider != null && hit.collider.IsTouching(collider);
    }

    private bool IsPlayerOnWall(bool ignoreGround = false)
    {
        return IsPointOnWall(transform.position + new Vector3(0, collider.bounds.extents.y * 0.52f, 0), ignoreGround) ||
               IsPointOnWall(transform.position - new Vector3(0, collider.bounds.extents.y * 0.24f, 0), ignoreGround) ||
               IsPointOnWall(transform.position - new Vector3(0, collider.bounds.extents.y * 0.8f, 0), ignoreGround) ||
               IsPointOnWall(transform.position - new Vector3(0, collider.bounds.extents.y * 1.45f, 0), ignoreGround);
    }

    private bool IsPointOnWall(Vector2 position, bool ignoreGround = false)
    {
        Debug.DrawLine(position, position + new Vector2(collider.bounds.extents.x * wallErrorThreshold, 0), Color.green);
        Debug.DrawLine(position, position - new Vector2(collider.bounds.extents.x * wallErrorThreshold, 0), Color.green);

        var rightHit = Physics2D.Linecast(position,
                                          position + new Vector2(collider.bounds.extents.x * wallErrorThreshold, 0),
                                          LayerMask.GetMask(LayerNames.Ground));
        var leftHit = Physics2D.Linecast(position,
                                         position - new Vector2(collider.bounds.extents.x * wallErrorThreshold, 0),
                                         LayerMask.GetMask(LayerNames.Ground));

        return ((rightHit.collider != null && rightHit.collider.IsTouching(collider)) ||
               (leftHit.collider != null && leftHit.collider.IsTouching(collider))) && (!IsPlayerOnGround() || ignoreGround);
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
        animator.SetBool("IsFighting", isFighting);
        animator.SetBool("IsHurt", isHurt);
    }

    private void PlayFightSound()
    {
        if (isFighting && !isPlayerHurt)
        {
            if (fightSoundAccumalator >= fightSoundLength)
            {
                SoundManagerScript.PlaySound(GetRandomFightClipName());
                fightSoundAccumalator = 0;
            }
            else 
            {
                fightSoundAccumalator++;
            }
        }
    }

    private string GetRandomFightClipName()
    {
        var fightClipNames = new List<string> { "punch", "hard punch", "slap", "hard slap" };
        var randomNumber = UnityEngine.Random.Range(0, fightClipNames.Count);
        return fightClipNames[randomNumber];
    }

    public void StartEncounter()
    {
        isPlayerHurt = false;
        isFighting = true;
        isMovementEnabled = false;
        rigidBody.gravityScale = 0f;
        rigidBody.velocity = Vector2.zero;
    }

    public void EndEncounter()
    {
        isPlayerHurt = false;
        isFighting = false;
        isMovementEnabled = true;
        rigidBody.gravityScale = 1f;
        DecreaseStaminaAfterBugFight();
    }

    public void ThrowUserInTheAirHurt()
    { 
        isPlayerHurt = true;
        GetComponent<Rigidbody2D>().velocity += new Vector2(Mathf.Sign(transform.localScale.x) * -1 * hurtVelocity, hurtVelocity);
        isFighting = false;
        isHurt = true;
        Hurt();
    }

    public float GetStaminaDifficultyFactor()
    {
        return Mathf.Clamp(currentStamina / (startStamina * 1f), 0.27f, 1);
    }
}
