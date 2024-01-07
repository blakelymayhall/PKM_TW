using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Debug_GridMovement : MonoBehaviour {

public float debug = 0;

    /* PUBLIC VARS */
    //*************************************************************************
    public MovementDirections facingDirection;
    public Vector2 tileSize = new Vector2(1f, 1f); 
    public Vector3 inputDirection = Vector3.zero;
    public Tilemap tilemap;
    public Vector2 nextTileCenter = Vector3.zero;
    public bool isMoving = false;
    public float playerSpeed;
    public bool isSprinting = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float playerSprintSpeed = 8f;
    private float playerWalkSpeed = 4f;
    private OW_CameraManager cameraManager;
    //*************************************************************************

    void Awake() {
        // Make sure the player object is not destroyed
        // when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        PlayerMovement();

        // Force camera to player
        
    }

    void PlayerMovement() {
        // If shift is held, sprint
        isSprinting = Input.GetButton("Run");
        playerSpeed = isSprinting ? playerSprintSpeed : playerWalkSpeed;

        // Get user input
        inputDirection = Vector3.zero;
        if (Input.GetButton("Horizontal")) {
            inputDirection.x = Input.GetAxis("Horizontal");
        }
        else if (Input.GetButton("Vertical")) {
            inputDirection.y = Input.GetAxis("Vertical");
        }
        inputDirection.Normalize();

        // Manage Input
        // Turn to face movement direction or 
        // initialize target location
        if (inputDirection != Vector3.zero && !isMoving) {
            if (inputDirection == OW_Globals.GetVector3FromDirection(facingDirection)) {
                Vector3 nextTilePosition = transform.position + 
                    new Vector3(inputDirection.x * tileSize.x, inputDirection.y * tileSize.y, 0f);
                Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
                nextTileCenter = tilemap.GetCellCenterWorld(nextTileCellPosition);
                cameraManager.playerPos = transform.position;
                isMoving = true;
            }
            else {
                facingDirection = OW_Globals.GetDirection(inputDirection);
            }
        }

        if (isMoving) {
            Vector2 newPosition = Vector3.MoveTowards(transform.position, 
                nextTileCenter, 
                Time.fixedDeltaTime*playerSpeed);
            GetComponent<Rigidbody2D>().MovePosition(newPosition);
            debug = Vector2.Distance(transform.position, nextTileCenter);
            if (debug < 1e-3) {
                GetComponent<Rigidbody2D>().MovePosition(nextTileCenter);
                if (inputDirection != Vector3.zero) {
                    facingDirection = OW_Globals.GetDirection(inputDirection);
                    Vector3 nextTilePosition = transform.position + 
                        new Vector3(inputDirection.x * tileSize.x, inputDirection.y * tileSize.y, 0f);
                    Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
                    nextTileCenter = tilemap.GetCellCenterWorld(nextTileCellPosition);
                    cameraManager.playerPos = transform.position;       
                }
                else {
                    isMoving = false;
                }
            }
        }
    }
}

