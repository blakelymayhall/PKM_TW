using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerMechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public Vector3 moveDirection;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float playerMoveSpeed = 33;
    private OW_CameraManager cameraManager;
    private List<RaycastHit2D> m_Contacts = new List<RaycastHit2D>();
    //*************************************************************************


    // Start is called before the first frame update
    void Start()
    {
        cameraManager = Camera.main.GetComponent<OW_CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        // Force camera to player
        cameraManager.playerPos = GetComponent<Rigidbody2D>().position;
    }

    /* PlayerMovement ()
     * This method updates gameobject's transform via rigid body motion 
     * Translates the gameobject based on WASD keys
     * 
     * Tests to see if the kinematic rigid body is 0.1 units from impacting
     * another rigid body. If so, halt movement as if struck a wall
     */
    void PlayerMovement()
    {
        // Initialize to no movement
        moveDirection.x = 0;
        moveDirection.y = 0;
        moveDirection.z = 0;

        // If horizontal pressed, move horizontal
        if (Input.GetButton("Horizontal"))
        {
            moveDirection.x = Input.GetAxis("Horizontal");
            moveDirection.y = 0;
            moveDirection.z = 0;
            moveDirection.Normalize();
        }

        // If vertical pressed, move vertical
        if (Input.GetButton("Vertical"))
        {
            moveDirection.x = 0;
            moveDirection.y = Input.GetAxis("Vertical");
            moveDirection.z = 0;
            moveDirection.Normalize();
        }

        // Send cast for all rigid bodies in our line-of-sight
        m_Contacts.Clear();
        GetComponent<Rigidbody2D>().
            Cast(moveDirection.normalized, m_Contacts, playerMoveSpeed);
        foreach (var contact in m_Contacts)
        {
            if (Vector2.Dot(contact.normal, moveDirection) < 0 &&
                contact.distance <= 0.1)
            {
                // Stop movement if within 0.1 units of another rigid body
                return;
            }
        }

        // No contact was found so move freely
        Vector3 target = moveDirection * playerMoveSpeed + transform.position;
        GetComponent<Rigidbody2D>().MovePosition(Vector3.
            Lerp(transform.position, target, Time.deltaTime));
        GetComponent<Rigidbody2D>().freezeRotation = true;
    }
}


