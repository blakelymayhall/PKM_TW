using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<MovementDirection> spinDirections = new();
    public float spinTime = 2f;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private NPC_Animator animator;

    private int waypointIndex = 1;
    private int spinIndex = 1;
    private MovementDirection facingDirection;
    //*************************************************************************

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        identity = GetComponent<NPC_Identity>();
        animator = GetComponent<NPC_Animator>();
    }

    void Update()
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

    private void Wander()
    {
        noInput = false;
        if(!isMoving)
        {
            bool facingMoveDirection = 
                (Vector2) OW_Globals.GetVector3FromDirection(facingDirection) == waypoints[waypointIndex];
            facingDirection = OW_Globals.GetDirection(waypoints[waypointIndex]);

            if(!facingMoveDirection)
            {
                GetComponent<NPC_Animator>().UpdateDirectionSprites(facingDirection);
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
        animator.DisplaySprite(facingDirection);
        
        spinIndex++;
        if(spinIndex >= spinDirections.Count)
        {
            spinIndex = 0;
        }

        isMoving = false;
    }
}
