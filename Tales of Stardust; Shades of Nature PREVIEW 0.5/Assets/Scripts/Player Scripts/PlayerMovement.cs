using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    Vector2 moveInput;
    public float runSpeed = 7.5f;
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

    private TouchingDirections touchingDirections;
    private int jumpCount = 0;
    public float jumpImpulse = 7.5f;
    private float fallGravityScale = 4f;

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

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
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
            rb2d.gravityScale = 1f; // Reset to default gravity scale when not falling
        }
    }

    private void FixedUpdate()
    {
        rb2d.velocity = new Vector2(moveInput.x * runSpeed, rb2d.velocity.y);       
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;

        SetFacingDirection(moveInput);
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
            // Reset jump count when grounded
            jumpCount = 0;
        }

        if (jumpCount == 0 && context.started)
        {
            // Perform initial jump
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpImpulse);
            jumpCount++;
            animator.SetTrigger(AnimationStrings.jump);
        }
        else if (jumpCount == 1 && context.started)
        {
            // Perform double jump
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpImpulse);
            jumpCount++;
            animator.SetTrigger(AnimationStrings.jump);
        }
        else if (context.canceled)
        {
            // Release jump button, stop applying upward force
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * 0.5f);
        }
    }
}