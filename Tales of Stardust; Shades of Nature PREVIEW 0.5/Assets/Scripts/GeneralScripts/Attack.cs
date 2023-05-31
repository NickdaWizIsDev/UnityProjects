using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int baseDamage = 10;
    public Vector2 knockback = Vector2.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if(damageable != null && !damageable.isInvincible)
        {
            bool gotHit = damageable.Hit(baseDamage, knockback);

            if (gotHit)
            {
                Debug.Log(collision.name + " hit for " + baseDamage);
            }
        }
    }
    public int GetDamage()
    {
        return baseDamage;
    }
}
