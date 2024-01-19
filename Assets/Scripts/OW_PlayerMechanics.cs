using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OW_PlayerMechanics : OW_MovingObject
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Tilemap tilemap;
    public bool isSpotted = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private OW_CameraManager cameraManager;
    private OW_PlayerAnimator playerAnimator;

    private Vector3 inputDirection = Vector3.zero;
    private bool startMove = false;
    private Vector3 facingDirection;
    //*************************************************************************

    void Awake()
    {
        // Make sure the player object is not destroyed
        // when a new scene is loaded
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponent<OW_PlayerAnimator>();
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    void Update()
    {
        if (!isSpotted)
        {
            GetUserInput();
        
            if (!noInput && !isMoving)
            {
                bool facingMoveDirection = facingDirection == inputDirection;
                facingDirection = inputDirection;

                if(!facingMoveDirection)
                {
                    playerAnimator.UpdateDirectionSprites(facingDirection);
                }

                StartCoroutine(CheckHeld());
                if (startMove) 
                {
                    Move(GetTargetTile(inputDirection, tilemap));
                }
            }

            cameraManager.playerPos = transform.position;
        }
        else 
        {
            isMoving = false;
            noInput = true;
        }
    }

    private void GetUserInput()
    {
        isSprinting = Input.GetButton("Run"); // Shift Key
    
        inputDirection = Vector3.zero;
        inputDirection.x = (int) Input.GetAxisRaw ("Horizontal");
        inputDirection.y = (int) Input.GetAxisRaw ("Vertical");
        if(inputDirection.x != 0)
        {
            inputDirection.y = 0;
        }
        inputDirection.Normalize();
        noInput = inputDirection == Vector3.zero;
    }    

    // Require the input key be held before moving 
    // This allows user to turn directions without moving
    private IEnumerator CheckHeld()
    {
        float startTime = Time.time;
        while (Time.time-startTime < 0.15f)
        {
            if (noInput) 
            {
                startMove = false;
                yield break;
            }
            yield return null;
        }
        startMove = true;
    }
}

