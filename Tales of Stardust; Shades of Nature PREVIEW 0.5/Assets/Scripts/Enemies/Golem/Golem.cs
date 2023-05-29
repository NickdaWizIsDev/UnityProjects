using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform playerDetection;

    private bool playerDetected;
    private Rigidbody2D rb;

    Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {        
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            StartCoroutine(StartMoving());
            animator.SetBool(AnimationStrings.isAwake, true);
        }        
    }

    private IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(1f); // Wait for 1 second before moving

        Move();
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
}
