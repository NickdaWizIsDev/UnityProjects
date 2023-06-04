using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float crawlSpeed = 5f;
    public bool isEnraged = false;

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public Detection detection;
    public Detection cliffDetection;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;
    Animator animator;
    public enum WalkableDirection { Right, Left }

    private WalkableDirection walkDirection;
    private Vector2 walkDirectionVector;
    public WalkableDirection WalkDirection
    {
        get { return walkDirection; }
        set
        {
            if (walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            walkDirection = value;
        }
    }

    public bool hasTarget = false;
    public bool HasTarget
    {
        get
        {
            return hasTarget;
        }
        private set
        {
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HasTarget = detection.detectedColliders.Count > 0;

        if(AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetection.detectedColliders.Count == 0)
        {
            FlipDirection();
        }

        if (HasTarget)
        {
            isEnraged = true;
            animator.SetBool(AnimationStrings.isEnraged, true);
        }

        if (CanMove)
        {
            rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);

            if (isEnraged)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector2 direction = new(player.transform.position.x - transform.position.x, 0f);
                    direction.Normalize();
                    rb.velocity = direction * crawlSpeed;

                    if (direction.x < 0f)
                    {
                        transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                    }
                    else if (direction.x > 0f)
                    {
                        transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                    }
                }
            }

        }
        else if (!CanMove)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (!damageable.IsAlive)
        {
            Invoke(nameof(Deactivate), damageable.deathClip.length);
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set properly.");
        }
    }
}
