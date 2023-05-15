using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    public float distanceFromCamera = 10f;
    public LayerMask groundLayer;

    private Camera mainCamera;
    private Vector2 targetPosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceFromCamera));
        }

        transform.position = new Vector3(targetPosition.x, targetPosition.y, 0);
    }
}
