using DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private BoxCollider2D touchingCol;
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];

    private DialogueTrigger trigger;

    private Damageable damageable;

    private Dash dash;

    private TouchingDirections touchingDirections;

    [SerializeField]
    private bool isGrounded;
    public bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        private set
        {
            isGrounded = value;
            animator.SetBool(AnimationStrings.isGrounded, value);
        }
    }

    Vector2 moveInput;
    public float runSpeed = 7.5f;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove && !InDialogue() && !touchingDirections.IsOnWall)
            {
                if (IsMoving)
                {
                    return runSpeed;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        set
        {

        }
    }
    private bool isMoving;
    public bool IsMoving 
    {
        get 
        {
            return isMoving;
        }
        
        private set
        {
            isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    private int jumpCount = 0;
    public float jumpImpulse = 7.5f;
    private readonly float fallGravityScale = 4f;

    private Animator animator;
    private bool isFacingRight = true;
    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
        private set
        {
            if (isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            isFacingRight = value;
        }
    }

    public AudioSource currentAudioSource;
    public AudioClip swordSwing;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        dash = GetComponent<Dash>();
        touchingDirections = GetComponent<TouchingDirections>();

        if(currentAudioSource == null)
            currentAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {

        animator.SetFloat(AnimationStrings.yVelocity, rb2d.velocity.y);

        if (rb2d.velocity.y < 0)
        {
            rb2d.gravityScale = fallGravityScale;
        }
        else
        {
            rb2d.gravityScale = 1f;
        }
    }

    private void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;

        if (!damageable.IsHit)
            rb2d.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb2d.velocity.y);
        if (dash.IsDashing)
        {
            rb2d.velocity = new Vector2(dash.dashSpeed * transform.localScale.x, rb2d.velocity.y);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!InDialogue())
        {
            moveInput = context.ReadValue<Vector2>();
            IsMoving = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if(moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (IsGrounded)
        {
            jumpCount = 0;
            Debug.Log("Jumps reset!");
        }

        if (jumpCount == 0 && context.started && CanMove && !InDialogue() && !dash.IsDashing)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpImpulse);
            jumpCount++;
            animator.SetTrigger(AnimationStrings.jump);
        }
        else if (jumpCount < 1 && context.started && CanMove && !InDialogue() && !dash.IsDashing)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpImpulse);
            jumpCount++;
            animator.SetTrigger(AnimationStrings.jump);
        }
        else if (context.canceled)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {

        if (context.started && !InDialogue() && !dash.IsDashing)
        {
            if(IsGrounded)
                animator.SetBool(AnimationStrings.atk, true);
            else
            {
                animator.SetBool(AnimationStrings.atk2, true);
            }
        }
    }

    public void PlayAttackSound()
    {
        GameObject audioObject = new("AttackAudio");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = swordSwing;

        audioSource.Play();

        Destroy(audioObject, swordSwing.length);

        currentAudioSource = audioSource;
    }

    private bool InDialogue()
    {
        if(trigger != null)
        {
            return trigger.DialogueActive();
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("dialogueTrigger"))
        {
            trigger = collision.gameObject.GetComponent<DialogueTrigger>();

            if (Input.GetKey(KeyCode.E))
                trigger.ActivateDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        trigger = null;
    }
}