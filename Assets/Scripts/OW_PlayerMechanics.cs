using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OW_PlayerMechanics : OW_MovingObject
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Tilemap tilemap;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private OW_PlayerAnimator playerAnimator;
    private OW_Player player;

    private bool initMode = false;
    private Vector3 inputDirection = Vector3.zero;
    private Vector3 facingDirection = Vector3.zero;
    private readonly float KEY_HOLD_TIME = 0.15f; 
    private readonly float TARGET_TOLERANCE = 1e-3f;
    //*************************************************************************

    void Awake()
    {
        sprintMoveTime = 0.2f;
        walkMoveTime = 0.4f;
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponent<OW_PlayerAnimator>();
        player = GetComponent<OW_Player>();
    }

    void Update()
    {
        switch(player.playerMode)
        {
            case OW_PlayerModes.STANDBY:
                GetUserInput();
                if (!noInput || isMoving)
                {
                    bool facingMoveDirection = facingDirection == inputDirection;
                    facingDirection = inputDirection;
                    if(!facingMoveDirection)
                    {
                        playerAnimator.UpdateDirectionSprites(facingDirection);
                    }
                    
                    player.playerMode = isMoving || facingMoveDirection ?  
                        OW_PlayerModes.MOVING : OW_PlayerModes.MOVE_DELAY;
                    initMode = true;
                }
                break;
            case OW_PlayerModes.MOVE_DELAY:
                GetUserInput();

                if (initMode)
                {
                    StartCoroutine(CheckHeld());
                    initMode = false;
                }
                break;
            case OW_PlayerModes.MOVING:
                GetUserInput();

                if (initMode)
                {
                    playerAnimator.ShowWalkingSprite();
                    Move(GetTargetTile(inputDirection, tilemap));
                    initMode = false;
                }
                break;
            case OW_PlayerModes.SPOTTED:
                isMoving = false;
                noInput = true;
                break;
            case OW_PlayerModes.ENGAGED:
                break;
            default:
                break;
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

    private IEnumerator CheckHeld()
    {
        float startTime = Time.time;
        while (Time.time-startTime < KEY_HOLD_TIME)
        {
            if (noInput) 
            {
                player.playerMode = OW_PlayerModes.STANDBY;
                yield break;
            }
            yield return null;
        }
        player.playerMode = OW_PlayerModes.MOVING;
        initMode = true;
    }

    protected override IEnumerator SmoothMovement(Vector2 target)
    {
        isMoving = true;

        while (true)
        {
            float inverseMoveTime = 1f / (isSprinting ? sprintMoveTime : walkMoveTime);
            Vector3 newPostion = Vector3.MoveTowards(
                rigidbody2D.position,
                target,
                inverseMoveTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPostion);

            float sqrRemainingDistance = (rigidbody2D.position - target).sqrMagnitude;
            if (sqrRemainingDistance < TARGET_TOLERANCE)
            {
                Vector3 nextTilePosition = target + new Vector2(
                    inputDirection.x * tileSize.x,
                    inputDirection.y * tileSize.y);
                Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
                bool facingMoveDirection = facingDirection == inputDirection;
                if (noInput || !facingMoveDirection || !CanMove(tilemap.GetCellCenterWorld(nextTileCellPosition)))
                {
                    rigidbody2D.MovePosition(target);
                    player.playerMode = OW_PlayerModes.STANDBY;
                    isMoving = !noInput;
                    yield break;
                }
                else 
                {
                    target = tilemap.GetCellCenterWorld(nextTileCellPosition);
                }
            }
            yield return null;
        }
    }
}

