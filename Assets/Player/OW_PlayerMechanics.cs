using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerMechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public MovementDirections facingDirection;
    public Vector3 inputDirection = Vector3.zero;
    public Vector3 movingDirection = Vector3.zero;
    public Vector3 initalPosition;
    public Vector3 target;
    public bool isMoving =false;
    public float playerSpeed;
    public float playerSprintSpeed = 16f;
    public float playerWalkSpeed = 12f;
    public bool isSprinting = false;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private OW_CameraManager cameraManager;
    private float stepLength = 0.8f;
    private float startTime;
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
        initalPosition = transform.position;
        target = transform.position;
        startTime = Time.time;
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        PlayerMovement();

        // Force camera to player
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    /* PlayerMovement ()
     * 
     * This method updates gameobject's transform via rigid body motion 
     * Translates the gameobject based on WASD keys input
     * 
     * Set player speed via checking if shift key is pressed
     * 
     * First time input is seen, check if player object is already facing that 
     * direction. If so, begin moving in that direction. Otherwise, first pass 
     * will rotate the player to that direction only. Establish target location
     * 
     * Once movement begins, move through to target. Once we reach target, 
     * establish new target
     * 
     * Once input is lost, finish move to target, then stop moving.
     * 
     * If new input is made while moving, finish move through original target, 
     * then establish new target.
     * 
     */
    void PlayerMovement()
    {
        // Set player speed
        if (Input.GetButton("Run"))
        {
            playerSpeed = playerSprintSpeed;
            isSprinting = true;
        }
        else
        {
            playerSpeed = playerWalkSpeed;
            isSprinting = false;
        }

        // Get user input direction
        inputDirection = Vector3.zero;
        if (Input.GetButton("Horizontal"))
        {
            inputDirection.x = Input.GetAxis("Horizontal");
            inputDirection.y = 0;
            inputDirection.Normalize();
        }
        if (Input.GetButton("Vertical"))
        {
            inputDirection.x = 0;
            inputDirection.y = Input.GetAxis("Vertical");
            inputDirection.Normalize();
        }

        if (inputDirection != Vector3.zero)
        {
            // Intend to move
            float elapsedTime = (Time.time - startTime);
            if (inputDirection ==
                OW_Globals.GetVector3FromDirection(facingDirection) &&
                elapsedTime > 0.22f)
            {
                // We are facing our move direction, proceed to move
                target = inputDirection * stepLength + initalPosition;
                isMoving = true;
            }
            else if(inputDirection !=
                OW_Globals.GetVector3FromDirection(facingDirection))
            {
                // We are not facing move direction.
                //
                // Either need to finish our current move or 
                // Spend one pass turning before moving

                if (!isMoving)
                {
                    startTime = Time.time;
                    facingDirection = OW_Globals.GetDirection(inputDirection);
                    return;
                }
            }
        }

        if(isMoving)
        {
            var test1 = Math.Abs(Math.IEEERemainder((double)target.x,
                (double)stepLength)) <= 1e-2;
            var test2 = Math.Abs(Math.IEEERemainder((double)target.y,
                (double)stepLength)) <= 1e-2;
            if (test1 && test2)
            {
                GetComponent<Rigidbody2D>().MovePosition(
                    Vector3.Lerp(transform.position, target,
                    playerSpeed * Time.fixedDeltaTime));
            }
        }

        if (Vector3.Distance(transform.position, target) <= 1e-3)
        {
            initalPosition = target;

            if(inputDirection == Vector3.zero)
            {
                isMoving = false;
            }

            if (inputDirection != Vector3.zero && inputDirection !=
                OW_Globals.GetVector3FromDirection(facingDirection))
            {
                facingDirection = OW_Globals.GetDirection(inputDirection);
            }
        }
    }
}
