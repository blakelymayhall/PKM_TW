using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPC_SpinDirection
{
    Up,
    Right,
    Down,
    Left
}

public class NPC_Mechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    // Wander Mechanics
    public bool isWander = false;

    // Spin Mechanics
    public bool isSpin = false;
    public List<NPC_SpinDirection> spinDirections =
        new List<NPC_SpinDirection>();
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float npcMoveSpeed = 0;
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
    void Update()
    {
        // DEBUG //
        // ///// //
        GetComponent<Rigidbody2D>().freezeRotation = true;

        Vector3 target = new Vector3(0, -1, 0)*npcMoveSpeed+transform.position;

        // Test to see if we've impacted something
        m_Contacts.Clear();
        GetComponent<Rigidbody2D>().
            Cast(target.normalized, m_Contacts, npcMoveSpeed);
        foreach (var contact in m_Contacts)
        {
            if (Vector2.Dot(contact.normal, target) < 0 &&
                contact.distance <= 0.1)
            {
                // Stop movement if within 0.1 units of another rigid body
                return;
            }
        }

        // No contact was found so move freely
        GetComponent<Rigidbody2D>().MovePosition(Vector3.
            Lerp(transform.position, target, Time.deltaTime));

        // END DEBUG //
        // /// ///// //
        
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
     * 4) PLAYER_SPOTTED - An enemy catches the player in the LOS and walks to them
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

    }

    /* Spin () 
     * 
     * The spin method is used for NPC's set to the spin setting. The 
     * spin method rotates the NPC through the list of angles defined in the 
     * parameters.
     * 
     * TODO Clean this up. This code is almost common with the player animator
     * AND I think that you could achieve this is less lines with switch maybe? 
     * 
     */
    void Spin()
    {
        float elapsedTime = Time.time - startTime;
        if(elapsedTime >= spinTime)
        {
            Vector2 currentDirection = transform.up;
            Vector2 targetDirection = currentDirection;
            if (spinDirections[spinIndex] == NPC_SpinDirection.Left)
            {
                // Face left
                targetDirection = Vector2.left;
            }
            else if (spinDirections[spinIndex] == NPC_SpinDirection.Right)
            {
                // Face right
                targetDirection = Vector2.right;
            }
            else if (spinDirections[spinIndex] == NPC_SpinDirection.Up)
            {
                // Face up
                targetDirection = Vector2.up;
            }
            else if (spinDirections[spinIndex] == NPC_SpinDirection.Down)
            {
                // Face down
                targetDirection = Vector2.down;
            }

            float angle =
                Vector2.SignedAngle(currentDirection, targetDirection);
            transform.Rotate(0f, 0f, angle);

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
