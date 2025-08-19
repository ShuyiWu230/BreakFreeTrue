using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target;
    Vector3 velocity = Vector3.zero;
    public bool IsCanFollow;

    [Range(0, 1)]
    public float smoothTime;

    public Vector3 positionoffset;
    private void Awake()
    {
        IsCanFollow = true;
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (IsCanFollow)
        {
            Vector3 targetPosition = target.position + positionoffset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }


    }
}
