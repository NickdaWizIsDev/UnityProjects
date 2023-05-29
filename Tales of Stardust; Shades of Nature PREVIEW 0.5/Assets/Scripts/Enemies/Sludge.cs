using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sludge : MonoBehaviour
{
    public float walkSpeed = 3f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Damageable damageable;

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

                if(value == WalkableDirection.Right)
                    {
                    walkDirectionVector = Vector2.right;
                    }
                else if(value == WalkableDirection.Left)
                    {
                    walkDirectionVector = Vector2.left;
                    }
            } walkDirection = value;
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if(touchingDirections.IsOnWall && touchingDirections.IsGrounded && damageable.IsAlive)
        {
            FlipDirection();
        }

        rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);

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
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if(WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is pretty much not poggers");
        }
    }
}
