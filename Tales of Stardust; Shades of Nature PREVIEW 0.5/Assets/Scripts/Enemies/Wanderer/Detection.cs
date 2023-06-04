using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new();

    Collider2D col;
    

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedColliders.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedColliders.Remove(collision);
    }
}
