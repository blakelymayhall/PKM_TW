using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Debug_GridMovement : MonoBehaviour
{
    public Tilemap tilemap;
    public Vector2 tileSize = new Vector2(1f, 1f); // Set the size of your tiles
    public Vector3Int currentTilePosition;  

    /* PUBLIC VARS */
    //*************************************************************************
    public Vector3 moveDirection;
    public Vector3 currentVelociy = Vector3.zero;
    public Vector3 targetPosition = Vector3.zero;
    public float test = 0f;
    public bool isSprinting = false;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float playerSpeed;
    private const float gridSize = 1f;
    private const float playerSprintSpeed = 3f;
    private const float playerWalkSpeed = 2f;
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
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Initialize to no movement
        moveDirection = Vector3.zero;

        PlayerMovement();

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
        if(Input.GetButton("Run") && !isSprinting)
        {
            playerSpeed = playerSprintSpeed;
            isSprinting = true;
        }
        else
        {
            playerSpeed = playerWalkSpeed;
            isSprinting = false;
        }

        // Handle input
        if (Input.GetButton("Horizontal"))
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = 0;
            moveDirection.Normalize();
        }
        if (Input.GetButton("Vertical"))
        {
            moveDirection.x = 0;
            moveDirection.y = Input.GetAxis("Vertical");
            moveDirection.Normalize();
        }

        // Rotate sprite
        OW_Globals.RotateSprite(gameObject, OW_Globals.GetDirection(moveDirection));

        // Move sprite
        Vector3 nextTilePosition = transform.position + 
            new Vector3(moveDirection.x * tileSize.x, moveDirection.y * tileSize.y, 0f);
        Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
        Vector3 nextTileCenter = tilemap.GetCellCenterWorld(nextTileCellPosition);
        
        Vector2 newPosition = Vector2.Lerp(
            GetComponent<Rigidbody2D>().position, 
            nextTileCenter, 
            1f / 0.01f * Time.deltaTime);
        GetComponent<Rigidbody2D>().MovePosition(newPosition);
    }
}


