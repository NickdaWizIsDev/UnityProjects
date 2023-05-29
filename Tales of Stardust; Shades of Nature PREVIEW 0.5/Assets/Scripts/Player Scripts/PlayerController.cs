using DialogueSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private DialogueTrigger trigger;

    Vector2 moveInput;
    public float runSpeed = 7.5f;
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove && !InDialogue())
            {
                if (IsMoving && !touchingDirections.IsOnWall)
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

    private TouchingDirections touchingDirections;
    private int jumpCount = 0;
    public float jumpImpulse = 7.5f;
    private readonly float fallGravityScale = 4f;

    Damageable damageable;

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

    public AudioSource curretAudioSource;
    public AudioClip swordSwing;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();

        if(curretAudioSource == null)
            curretAudioSource = GetComponent<AudioSource>();
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
        if(!damageable.IsHit)
            rb2d.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb2d.velocity.y);
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
        if (touchingDirections.IsGrounded)
        {
            jumpCount = 0;
            Debug.Log("Jumps reset!");
        }

        if (jumpCount == 0 && context.started && CanMove && !InDialogue())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpImpulse);
            jumpCount++;
            animator.SetTrigger(AnimationStrings.jump);
        }
        else if (jumpCount < 1 && context.started && CanMove && !InDialogue())
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

        if (context.started && !InDialogue())
        {
            if(touchingDirections.IsGrounded)
                animator.SetTrigger(AnimationStrings.atk);
            else
            {
                animator.SetTrigger(AnimationStrings.atk2);
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

        curretAudioSource = audioSource;
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