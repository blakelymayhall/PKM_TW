using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerMechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Vector3 moveDirection;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float playerMoveSpeed = 33;
    private OW_CameraManager cameraManager;
    //*************************************************************************


    // Start is called before the first frame update
    void Start()
    {
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    // Allows player movement via WASD
    void PlayerMovement()
    {
        // Initialize to no movement
        moveDirection.x = 0;
        moveDirection.y = 0;
        moveDirection.z = 0;

        // If horizontal pressed, move horizontal
        if (Input.GetButton("Horizontal"))
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = 0;
            moveDirection.z = 0;
            moveDirection.Normalize();
        }

        // If vertical pressed, move vertical
        if (Input.GetButton("Vertical"))
        {
            moveDirection.x = 0;
            moveDirection.y = Input.GetAxis("Vertical");
            moveDirection.z = 0;
            moveDirection.Normalize();
        }

        // Move; Don't allow rotation
        Vector3 target = moveDirection*playerMoveSpeed+transform.position;
        GetComponent<Rigidbody2D>().MovePosition(Vector3.
            Lerp(transform.position,target,Time.deltaTime));
        GetComponent<Rigidbody2D>().freezeRotation = true;

        // Force camera to player
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }
}
