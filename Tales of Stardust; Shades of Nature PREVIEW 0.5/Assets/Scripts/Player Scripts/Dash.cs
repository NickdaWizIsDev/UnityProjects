using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb2d;
    PlayerController controller;

    [SerializeField]
    private bool isDashing = false;
    public bool IsDashing
    {
        get
        {
            return isDashing;
        }
        private set
        {
            isDashing = value;
            animator.SetBool(AnimationStrings.isDashing, value);
        }
    }

    public float dashDistance = 3f;
    public float dashDuration = 0.25f;
    public float dashSpeed = 35f;
    public float dashCooldown = 1f;
    private float dashStartTime;

    public AudioSource currentAudioSource;
    public AudioClip dash;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();

        if (currentAudioSource == null)
            currentAudioSource = GetComponent<AudioSource>();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.started && !IsDashing && Time.time >= dashStartTime + dashCooldown)
        {
            StartCoroutine(StardustDash(transform.localScale.x));
        }
    }

    private IEnumerator StardustDash(float direction)
    {
        IsDashing = true;
        dashStartTime = Time.time;

        float normalSpeed = controller.runSpeed;
        controller.runSpeed = dashSpeed;

        // Determine the dash direction based on the player's facing direction
        Vector2 dashDirection = controller.IsFacingRight ? Vector2.right : Vector2.left;
        Vector2 dashVelocity = dashDirection * (dashDistance / dashDuration);
        float distanceTraveled = 0f;

        while (distanceTraveled < dashDistance)
        {
            rb2d.velocity = dashVelocity;
            distanceTraveled += Mathf.Abs(dashVelocity.x) * Time.deltaTime;
            yield return null;
        }

        controller.runSpeed = normalSpeed;
        IsDashing = false;
    }
    public void PlayDashSound()
    {
        GameObject audioObject = new("DashAudio");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();

        audioSource.clip = dash;

        audioSource.Play();

        Destroy(audioObject, dash.length);

        currentAudioSource = audioSource;
    }
}
