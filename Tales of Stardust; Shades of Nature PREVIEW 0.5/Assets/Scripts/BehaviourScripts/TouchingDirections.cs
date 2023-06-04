using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingDirections : MonoBehaviour
{

    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;

    CapsuleCollider2D touchingCol;
    Animator animator;

    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];

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

    [SerializeField]
    private bool isOnWall;

    private Vector2 WallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnWall
    {
        get
        {
            return isOnWall;
        }
        private set
        {
            isOnWall = value;
            animator.SetBool(AnimationStrings.isOnWall, value);
        }
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(WallCheckDirection, castFilter, wallHits, wallDistance) > 0;
    }
}
