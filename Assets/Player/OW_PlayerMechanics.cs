using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerMechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Vector3 moveDirection;
    public bool isSprinting = false;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float playerSpeed;
    public Vector3 target1;
    public Vector3 target2;
    public float playerSprintSpeed = 16f;
    public float playerWalkSpeed = 12f;
    private OW_CameraManager cameraManager;
    private List<RaycastHit2D> m_Contacts = new List<RaycastHit2D>();
    //*************************************************************************

    void Awake()
    {
        // Make sure the player object is not destroyed
        // when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        target1 = transform.position;
        target2 = transform.position;
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDirection = Vector3.zero;

        if (!isSpotted)
        {
            PlayerMovement();
        }

        // Force camera to player
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    /* PlayerMovement ()
     * This method updates gameobject's transform via rigid body motion 
     * Translates the gameobject based on WASD keys
     * 
     * Tests to see if the kinematic rigid body is 0.1 units from impacting
     * another rigid body. If so, halt movement as if struck a wall
     */
    void PlayerMovement()
    {
        // If shift is held, sprint
        if(Input.GetButton("Run"))
        {
            playerSpeed = playerSprintSpeed;
            isSprinting = true;
        }
        else
        {
            playerSpeed = playerWalkSpeed;
            isSprinting = false;
        }

        // If horizontal pressed, move horizontal
        if (Input.GetButton("Horizontal"))
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = 0;
            moveDirection.Normalize();
        }

        // If vertical pressed, move vertical
        if (Input.GetButton("Vertical"))
        {
            moveDirection.x = 0;
            moveDirection.y = Input.GetAxis("Vertical");
            moveDirection.Normalize();
        }

        if (moveDirection == Vector3.zero)
        {
            // Not actively moving
            if (Vector3.Distance(transform.position, target2) >= 1e-3)
            {
                var test3 = Math.Abs(Math.IEEERemainder((double)target2.x, 0.8d)) <= 1e-3;
                var test4 = Math.Abs(Math.IEEERemainder((double)target2.y, 0.8d)) <= 1e-3;
                if (test3 && test4)
                {
                    GetComponent<Rigidbody2D>().MovePosition(
                        Vector3.Lerp(transform.position, target2,
                        playerSpeed * Time.fixedDeltaTime));
                }
                return;
            }
            else
            {
                // snap to target and be done moving
                GetComponent<Rigidbody2D>().MovePosition(target2);
                transform.position = target2;
                target1 = target2;
                return;
            }

        }
        else
        {


            // No contact was found so move freely
            target2 = moveDirection * 0.8f + target1;

            var test1 = Math.Abs(Math.IEEERemainder((double)target2.x, 0.8d)) <= 1e-2;
            var test2 = Math.Abs(Math.IEEERemainder((double)target2.y, 0.8d)) <= 1e-2;
            if (test1 && test2)
            {
                GetComponent<Rigidbody2D>().MovePosition(
                    Vector3.Lerp(transform.position, target2,
                    playerSpeed * Time.fixedDeltaTime));
            }
        }

        //OW_Globals.RotateSprite(gameObject,
    //OW_Globals.GetDirection(moveDirection));

        if (Vector3.Distance(transform.position, target2) <= 1e-3)
        {
            target1 = target2;
        }
    }
}


