using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPC_Mechanics : OW_MovingObject
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Tilemap tilemap;
    public NPC_Identity identity;

    public List<Vector2> waypoints = new();
    public Vector3 moveDirection = Vector3.zero;
    public List<Vector2> spinDirections = new();
    public float spinTime = 2f;
    public int spottingDistance = 9;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private NPC_Animator animator;
    private GameObject player;

    private int waypointIndex = 1;
    private int spinIndex = 1;
    private bool playerSpotted = false;
    //*************************************************************************

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    protected override void Start()
    {
        base.Start();
        identity = GetComponent<NPC_Identity>();
        animator = GetComponent<NPC_Animator>();

        SnapToGrid(tilemap);
    }

    void Update()
    {
        if (playerSpotted || PlayerInLOS())
        {
            identity.npc_movestyle = NPC_MoveStyle.None;
            player.GetComponent<OW_Player>().playerMode = OW_PlayerModes.SPOTTED;
            MoveToPlayer();
        }
        else
        {
            switch(identity.npc_movestyle)
            {
                case NPC_MoveStyle.Wander:
                    Wander();
                    break;
                case NPC_MoveStyle.Spin:
                    if(!isMoving)
                    {
                        StartCoroutine(Spin());
                    }
                    break;
                default:
                    break;
            } 
        }
    }

    private void Wander()
    {
        noInput = false;
        if(!isMoving)
        {
            bool facingMoveDirection = facingDirection == waypoints[waypointIndex];
            facingDirection = waypoints[waypointIndex];

            if(!facingMoveDirection)
            {
                animator.UpdateDirectionSprites(facingDirection);
                if (PlayerInLOS())
                {
                    return;
                }
            }

            Move(GetTargetTile(waypoints[waypointIndex], tilemap));
                    
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }

    private IEnumerator Spin()
    {
        isMoving = true;

        float startTime = Time.time;
        while (Time.time-startTime < spinTime)
        {
            yield return null;
        }
        facingDirection = spinDirections[spinIndex];
        animator.UpdateDirectionSprites(facingDirection);
        if (PlayerInLOS())
        {
            isMoving = false;
            yield break;
        }
        
        spinIndex++;
        if(spinIndex >= spinDirections.Count)
        {
            spinIndex = 0;
        }

        isMoving = false;
    }

    private bool PlayerInLOS() 
    {
        Vector2 target = (Vector2)transform.position+spottingDistance*tileSize*facingDirection;

        // Cast a ray in the facing direction to confirm that 
        // nothing rigid is in our path. Disable this objects collider.
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target);
        GetComponent<BoxCollider2D>().enabled = true;
        
        playerSpotted = hit.transform != null && hit.collider.gameObject == player;
        return playerSpotted;
    }

    private void MoveToPlayer()
    {
        noInput = false;
        Vector2 oppositeFacingDirection = -1*facingDirection;
        Vector3Int playerTile = tilemap.WorldToCell(player.transform.position);
        Vector2 target = (Vector2)tilemap.GetCellCenterWorld(playerTile)+tileSize*oppositeFacingDirection;
        Move(target);
        noInput = true;
    }
}
