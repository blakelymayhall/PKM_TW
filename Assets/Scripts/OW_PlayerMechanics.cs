using UnityEngine;
using UnityEngine.Tilemaps;

public class OW_PlayerMechanics : OW_MovingObject
{
    /* PUBLIC VARS */
    //*************************************************************************
    public MovementDirections facingDirection;
    public Tilemap tilemap;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private Vector3 inputDirection = Vector3.zero;
    private OW_CameraManager cameraManager;
    //*************************************************************************

    void Awake()
    {
        // Make sure the player object is not destroyed
        // when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    void Update()
    {
        SetSprint();
        
        GetUserInput();
        if (!isMoving && inputDirection != Vector3.zero)
        {
            facingDirection = OW_Globals.GetDirection(inputDirection);
            Move(GetTargetTile(inputDirection, tilemap));
        }   
        
        cameraManager.playerPos = transform.position;
    }

    void SetSprint() 
    {
        isSprinting = Input.GetButton("Run"); // Shift Key
    }

    void GetUserInput()
    {
        inputDirection = Vector3.zero;
        inputDirection.x = (int) Input.GetAxisRaw ("Horizontal");
        inputDirection.y = (int) Input.GetAxisRaw ("Vertical");
        if(inputDirection.x != 0)
        {
            inputDirection.y = 0;
        }
        inputDirection.Normalize();
    }    
}

