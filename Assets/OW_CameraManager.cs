using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_CameraManager : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Vector3 playerPos;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3f;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Use fixed update to prevent camera jumps and glitchy behavior
    private void FixedUpdate()
    {
        Vector3 targetPosition = playerPos;
        targetPosition.z = transform.position.z-10f;

        transform.position = Vector3.SmoothDamp(transform.position,
            targetPosition, ref velocity, smoothTime);
    }
}
