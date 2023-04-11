using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Mechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    // Wander Mechanics
    public bool isWander = false;
    public List<Vector3> waypoints = new List<Vector3>();
    public Vector3 target = new Vector3();

    // Spin Mechanics
    public bool isSpin = false;
    public List<MovementDirections> spinDirections =
        new List<MovementDirections>();
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    // Wander mechanics
    private Vector3 moveDirection = new Vector3();
    private int waypointIndex = 1;
    private float npcMoveSpeed = 2;
    private List<RaycastHit2D> m_Contacts = new List<RaycastHit2D>();

    // Spin Mechanics
    private int spinIndex = 1;
    private float startTime;
    private float spinTime = 2f;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
            moveDirection = (waypoints[waypointIndex] -
                transform.position).normalized;

            // Test to see if we've impacted something
            m_Contacts.Clear();
            GetComponent<Rigidbody2D>().
                Cast(moveDirection, m_Contacts, npcMoveSpeed);
            foreach (var contact in m_Contacts)
            {
                if (Vector2.Dot(contact.normal, moveDirection) < 0 &&
                    contact.distance <= npcMoveSpeed*Time.fixedDeltaTime*1.1)
                {
                    // Stop movement if within 0.1 units of another rigid body
                    return;
                }
            }

            // No contact was found so move freely
            target = moveDirection * npcMoveSpeed + transform.position;
            GetComponent<Rigidbody2D>().MovePosition(Vector3.
                Lerp(transform.position, target, Time.fixedDeltaTime));
            OW_Globals.RotateSprite(gameObject,
                OW_Globals.GetDirection((waypoints[waypointIndex] -
                transform.position).normalized));
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
     * the wander setting, rotate, or static setting. The 
     * PlayerSpotted method moves the NPC adjacent to the player prior to 
     * starting dialouge and battle.
     * 
     * This method should only operate once during the game unless a method for
     * replaying battles is established
     * 
     */
    void PlayerSpotted()
    {

    }
}
