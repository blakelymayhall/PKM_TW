using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Mechanics : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************

    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private float npcMoveSpeed = 0;
    private List<RaycastHit2D> m_Contacts = new List<RaycastHit2D>();
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
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
    }
}
