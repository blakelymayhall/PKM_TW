using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OW_PlayerMechanics : MonoBehaviour
{

    /* PUBLIC VARS */
    //*************************************************************************
    public MovementDirections facingDirection;
    public Tilemap tilemap;
    public Vector2 targetLocation = Vector3.zero;
    public bool isMoving = false;
    public float playerSpeed;
    public bool isSprinting = false;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private Vector3 inputDirection = Vector3.zero;
    private float playerSprintSpeed = 8f;
    private float playerWalkSpeed = 4f;
    private Vector2 tileSize = new Vector2(1f, 1f);
    private OW_CameraManager cameraManager;
    private float startTime;
    //*************************************************************************

    void Awake()
    {
        // Make sure the player object is not destroyed
        // when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
        startTime = Time.time;
    }

    void FixedUpdate()
    {
        SetSprint();
        Vector3 newInputDirection = GetUserInput();
        UpdateFacingDirection(newInputDirection);
        StartMoveFromStationary();
        MovePlayer();
    }

    void SetSprint() 
    {
        // If shift is held, sprint
        isSprinting = Input.GetButton("Run");
        playerSpeed = isSprinting ? playerSprintSpeed : playerWalkSpeed;
    }

    Vector3 GetUserInput()
    {
        // Get user input
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetButton("Horizontal"))
        {
            inputDirection.x = Input.GetAxis("Horizontal");
        }
        else if (Input.GetButton("Vertical"))
        {
            inputDirection.y = Input.GetAxis("Vertical");
        }
        inputDirection.Normalize();
        return inputDirection;
    }

    void UpdateFacingDirection(Vector3 newInputDirection)
    {
        if (inputDirection != newInputDirection) 
        {
            if (!isMoving)
            {
                // Start stationary timer to allow for single-tap direction change
                startTime = Time.time;
            }
            facingDirection = OW_Globals.GetDirection(newInputDirection);
            inputDirection = newInputDirection;
        }
    }

    void StartMoveFromStationary()
    {
        if (inputDirection != Vector3.zero && !isMoving)
        {
            // Start move after delay to allow for single-tap direction change
            float timeStationary = Time.time - startTime;
            if (timeStationary > 0.11f)
            {
                targetLocation = GetTargetTile();
                isMoving = true;
            }
        }
    }

    void MovePlayer()
    {
        if (isMoving)
        {
            Vector2 newPosition = Vector3.MoveTowards(transform.position,
                targetLocation,
                Time.fixedDeltaTime * playerSpeed);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
            ResetIfTargetReached();
        }
    }

    Vector3 GetTargetTile()
    {
        Vector3 nextTilePosition = transform.position +
            new Vector3(inputDirection.x * tileSize.x, inputDirection.y * tileSize.y, 0f);
        Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
        return tilemap.GetCellCenterWorld(nextTileCellPosition);
    }

    void ResetIfTargetReached() 
    {
        if (Vector2.Distance(transform.position, targetLocation) < 1e-3)
        {
            GetComponent<Rigidbody2D>().MovePosition(targetLocation);
            if (inputDirection != Vector3.zero)
            {
                targetLocation = GetTargetTile();
                cameraManager.playerPos = transform.position;
            }
            else
            {
                isMoving = false;
            }
        }
    }
}

