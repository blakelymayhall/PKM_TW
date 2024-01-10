using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPC_Mechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    // Wander Mechanics
    public bool isWander = false;
    public List<Vector3> waypoints = new List<Vector3>();
    public Vector3 moveDirection = new Vector3();

    // Spin Mechanics
    public bool isSpin = false;
    public List<MovementDirection> spinDirections =
        new List<MovementDirection>();

    // Player Spotting Mechanics
    public float spotDistance = 3f;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    // Wander mechanics
    private int waypointIndex = 1;
    private float npcMoveSpeed = 2;
    private List<RaycastHit2D> m_Contacts = new List<RaycastHit2D>();

    // Spin Mechanics
    private int spinIndex = 1;
    private float startTime;
    private float spinTime = 2f;

    // Player Spotting Mechanics
    private bool playerSpotted = false;
    private bool playerEngaged = false;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        moveDirection = Vector3.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GetComponent<NPC_Identity>().npc_types.Contains(NPC_Type.Enemy)
            && !playerEngaged)
        {
            PlayerSpotted();
        }
            
        if (isWander)
        {
            Wander();
        }
        
        if (isSpin)
        {
            Spin();
        }
    }

    /* NPC MOVEMENT 
     * 
     * THREE OPTIONS, ONE TRIGGERED OPTION:
     * 
     * 1) WANDER - Wanders through waypoints determined in class parameters
     * 2) SPIN - Rotates through directions defined in class parameters 
     * 3) STATIC - Uncoded, just looks in one direction
     *  
     * 4) PLAYER_SPOTTED - An enemy catches the player in the LOS and walks to 
     * them
     * 
     */

    /* Wander () 
     * 
     * The wander method is used for NPC's set to the wander setting. The 
     * wander method moves the NPC through the list of targets defined in the 
     * class parameters.
     * 
     * The NPC rotates to its move direction.
     * 
     */
    void Wander()
    {
        if(Vector3.Distance(transform.position, waypoints[waypointIndex])
            >= 0.1)
        {
            NPC_Move(waypoints[waypointIndex]);
        }
        else
        {
            waypointIndex++;
            if (waypointIndex >= waypoints.Count)
            {
                waypointIndex = 0;
            }
        }
    }

    /* Spin () 
     * 
     * The spin method is used for NPC's set to the spin setting. The 
     * spin method rotates the NPC through the list of angles defined in the 
     * parameters.
     * 
     */
    void Spin()
    {
        float elapsedTime = Time.time - startTime;
        if(elapsedTime >= spinTime)
        {
            OW_Globals.RotateSprite(gameObject, spinDirections[spinIndex]);
            spinIndex++;
            if(spinIndex >= spinDirections.Count)
            {
                spinIndex = 0;
            }
            startTime = Time.time;
        }

    }

    /* PlayerSpotted () 
     * 
     * The PlayerSpotted method is used for all enemy NPC's set to either 
     * the wander setting, rotate, or static setting. It checks its LOS for
     * the lpayer and then the  moves the NPC adjacent to the player prior to 
     * starting dialouge and battle.
     * 
     * This method should only fully operate once per NPC during the game
     * unless a method for replaying battles is established
     * 
     */
    void PlayerSpotted()
    {
        Vector3 facingDirection = gameObject.transform.up;

        // Test to see if player is in our facing direction
        m_Contacts.Clear();
        GetComponent<Rigidbody2D>().
            Cast(facingDirection, m_Contacts, spotDistance);
        RaycastHit2D playerHit = m_Contacts.SingleOrDefault(x =>
            x.collider.gameObject.name == "OW_Player");
        if (playerHit.collider != null)
        {
            // found player 
            GameObject playerObject = playerHit.collider.gameObject;
            var crossP = Vector3.Cross(
                playerObject.transform.position-transform.position,
                facingDirection);

            // test to see if looking at center of player
            if (Vector3.Magnitude(crossP) <= 0.1)
            {
                // Caught center of player. Stop player and wait for 1.3 sec
                // then walk up to them
                playerSpotted = true;

                // Start timer
                if (playerObject.GetComponent<OW_PlayerMechanics>().
                    isSpotted == false)
                {
                    startTime = Time.time;
                }

                isWander = false;
                isSpin = false;
                playerObject.GetComponent<OW_PlayerMechanics>().
                    isSpotted = true;

                // Wait 1.3 seconds from time of spot
                if (Time.time - startTime <= 1.3f)
                    return;

                // This may have issue with moving out-of plane if player
                // spotted code doesn't get the exact center of collider

                NPC_Move(playerObject.transform.position);
                if(playerEngaged)
                {
                    moveDirection = Vector3.zero;
                    GetComponent<SpriteRenderer>().color = Color.cyan;
                    // TODO initiate dialogue
                    // TODO load battle scene
                }
            }
            else
            {
                playerSpotted = false;
            }
        }
        else
        {
            playerSpotted = false;
        }
    }


    /* NPC_Move()
     * Moves the NPC in the direction of the waypoint
     * 
     * Tests to see if the kinematic rigid body is 0.1 units from impacting
     * another rigid body.If so, halt movement as if struck a wall
     */
    void NPC_Move(Vector3 waypoint)
    {
        moveDirection = (waypoint - transform.position).normalized;

        // Test to see if we've impacted something
        m_Contacts.Clear();
        GetComponent<Rigidbody2D>().
            Cast(moveDirection, m_Contacts, npcMoveSpeed);
        foreach (var contact in m_Contacts)
        {
            if (Vector2.Dot(contact.normal, moveDirection) < 0 &&
                contact.distance <= npcMoveSpeed * Time.fixedDeltaTime * 1.1)
            {
                playerEngaged = playerSpotted;
                return;
            }
        }

        // No contact was found so move freely
        playerEngaged = false;
        Vector3 target = moveDirection * npcMoveSpeed + transform.position;
        GetComponent<Rigidbody2D>().MovePosition(Vector3.
            Lerp(transform.position, target, Time.fixedDeltaTime));
        OW_Globals.RotateSprite(gameObject, OW_Globals.GetDirection(
            (waypoint - transform.position).normalized));
    }
}
