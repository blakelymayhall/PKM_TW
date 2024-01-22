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
    private OW_PlayerAnimator playerAnimator;

    private Vector3 inputDirection = Vector3.zero;
    private bool startMove = false;
    private Vector3 facingDirection;
    private readonly float KEY_HOLD_TIME = 0.15f; 
    //*************************************************************************

    void Awake()
    {
        sprintMoveTime = 0.19f;
        walkMoveTime = 0.25f;
        DontDestroyOnLoad(gameObject);
    }

    protected override void Start()
    {
        base.Start();
        playerAnimator = GetComponent<OW_PlayerAnimator>();
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

    private IEnumerator CheckHeld()
    {
        float startTime = Time.time;
        while (Time.time-startTime < KEY_HOLD_TIME)
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
            if (sqrRemainingDistance < 1e-3)
            {
                bool facingMoveDirection = facingDirection == inputDirection;
                if (noInput || !facingMoveDirection)
                {
                    rigidbody2D.MovePosition(target);
                    isMoving = false;
                    yield break;
                }
                else 
                {
                    Vector3 nextTilePosition = target + new Vector2(
                        inputDirection.x * tileSize.x,
                        inputDirection.y * tileSize.y);
                    Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
                    target = tilemap.GetCellCenterWorld(nextTileCellPosition);
                }
            }
            yield return null;
        }
    }
}

