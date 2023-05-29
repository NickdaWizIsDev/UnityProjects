using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elemental : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;
    Damageable damageable;
    public AudioSource audioSource;
    public AudioClip shootClip;

    public GameObject laserPrefab;       // Reference to the laser prefab
    public Transform firePoint;          // The position from which the laser is fired
    public float detectionRange = 5f;

    public float cooldownDuration = 5f;  // Cooldown duration between shots
    private float cooldownTimer;         // Timer for cooldown

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        damageable = GetComponent<Damageable>();
    }

    private void Start()
    {
        cooldownTimer = 5f;              // Start on cooldown
    }

    private void FixedUpdate()
    {
        if (!damageable.IsAlive)
        {
            Invoke(nameof(ThyEndIsNow), damageable.deathClip.length);
        }
    }

    private void Update()
    {
        if (cooldownTimer <= 0f)
        {
            if (PlayerInRange(detectionRange))
            {
                animator.SetTrigger(AnimationStrings.isShooting);
            }
            cooldownTimer = cooldownDuration; // Set the cooldown timer back to 5
        }

        cooldownTimer -= Time.deltaTime; // Decrease the cooldown timer
    }

    public void ShootLaser()
    {
        Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        if (audioSource != null && shootClip != null)
        {
            audioSource.clip = shootClip;
            audioSource.Play();
        }
    }

    private bool PlayerInRange(float range)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            return distance <= range;
        }

        return false;
    }

    private void ThyEndIsNow()
    {
        gameObject.SetActive(false);
    }
}
