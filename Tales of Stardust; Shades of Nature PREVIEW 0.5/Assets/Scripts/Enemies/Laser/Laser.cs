using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private Rigidbody2D rb;

    public float shotSpeed = 10f;
    public int baseDamage = 25;
    public Vector2 knockback = Vector2.zero;

    public string groundTag = "Ground";
    public string playerTag = "Player";

    private Vector2 playerDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerDirection = FindPlayerDirection(); // Calculate the player direction once
    }

    private void Start()
    {
        rb.AddForce(playerDirection * shotSpeed, ForceMode2D.Impulse); // Apply the initial force
    }

    private Vector2 FindPlayerDirection()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();

        return direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable != null)
        {
            bool gotHit = damageable.Hit(baseDamage, knockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + baseDamage);
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag(groundTag))
        {
            Destroy(gameObject); // Destroy the bullet
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(groundTag))
        {
            Destroy(gameObject); // Destroy the bullet
        }
    }
}
