using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothDampTime = 0.2f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothDampTime);
        transform.position = smoothedPosition;
    }
}
