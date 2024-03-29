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
    private Vector2 inputDirection = Vector2.zero;

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
        SnapToGrid(tilemap);
    }

    void Update()
    {
        switch (player.playerMode)
        {
            case OW_PlayerModes.STANDBY:
                GetUserInput();
                if (!noInput || isMoving)
                {
                    bool facingMoveDirection = facingDirection == inputDirection;
                    facingDirection = inputDirection;
                    if (!facingMoveDirection)
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

                if (initMode && Move(GetTargetTile(inputDirection, tilemap)))
                {
                    playerAnimator.ShowWalkingSprite();
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

        inputDirection = Vector2.zero;
        inputDirection.x = (int)Input.GetAxisRaw("Horizontal");
        inputDirection.y = (int)Input.GetAxisRaw("Vertical");
        if (inputDirection.x != 0)
        {
            inputDirection.y = 0;
        }
        inputDirection.Normalize();
        noInput = inputDirection == Vector2.zero;
    }

    private IEnumerator CheckHeld()
    {
        float startTime = Time.time;
        while (Time.time - startTime < KEY_HOLD_TIME)
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
            if (!isMoving)
            {
                yield break;
            }

            float inverseMoveTime = 1f / (isSprinting ? sprintMoveTime : walkMoveTime);
            Vector2 newPostion = Vector2.MoveTowards(
                rigidbody2D.position,
                target,
                inverseMoveTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPostion);

            float sqrRemainingDistance = (rigidbody2D.position - target).sqrMagnitude;
            if (sqrRemainingDistance < TARGET_TOLERANCE)
            {
                rigidbody2D.MovePosition(target);
                Vector2 nextTilePosition = target + new Vector2(
                    inputDirection.x * tileSize.x,
                    inputDirection.y * tileSize.y);
                Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);

                bool facingMoveDirection = facingDirection == inputDirection;

                // Handle stop moving
                bool doNotMove = noInput || !CanMove(tilemap.GetCellCenterWorld(nextTileCellPosition));
                if (doNotMove || !facingMoveDirection)
                {
                    isMoving = !doNotMove;
                    player.playerMode = OW_PlayerModes.STANDBY;
                    yield break;
                }

                target = (Vector2)tilemap.GetCellCenterWorld(nextTileCellPosition);
            }
            yield return null;
        }
    }
}

