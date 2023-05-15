using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 400f;
    public float dashDistance = 3f;
    public float dashDuration = 0.25f;
    public float dashSpeed = 20f;
    public float dashCooldown = 1f;
    public LayerMask groundLayer;
    private bool isGrounded;
    private Rigidbody2D rb2d;
    private Animator animator;
    private bool isDashing = false;
    private float dashStartTime;
    private int jumpCount = 0;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2(horizontalInput * speed, rb2d.velocity.y);

        if (horizontalInput != 0)
        {
            animator.SetBool("IsRunning", true);
            if (horizontalInput > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (horizontalInput < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        isGrounded = Physics2D.Raycast(transform.position, -Vector2.up, 1.0f, groundLayer);

        if (isGrounded)
        {
            jumpCount = 0;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jumpCount++;
            animator.SetTrigger("Jump");
        }
        else if (jumpCount < 1 && Input.GetButtonDown("Jump"))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jumpCount++;
            animator.SetTrigger("Jump");
        }

        if (!isDashing && Time.time >= dashStartTime + dashCooldown && Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(Dash(transform.localScale.x));
        }
    }

    private IEnumerator Dash(float direction)
    {
        isDashing = true;
        dashStartTime = Time.time;

        float originalSpeed = speed;
        speed = dashSpeed;

        Vector2 dashVelocity = new Vector2(direction * dashDistance / dashDuration, 0f);
        float distanceTraveled = 0f;

        while (distanceTraveled < dashDistance)
        {
            rb2d.velocity = dashVelocity;
            distanceTraveled += Mathf.Abs(dashVelocity.x) * Time.deltaTime;
            yield return null;
        }

        speed = originalSpeed;
        isDashing = false;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0 && !collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0 && !collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = false;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0 && !collision.gameObject.CompareTag("Enemy"))
        {
            isGrounded = true;
        }
    }
}
