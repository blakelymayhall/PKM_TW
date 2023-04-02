using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : MonoBehaviour
{
    /* PRIVATE VARS */
    //*************************************************************************
    private SpriteRenderer spriteRenderer;
    private List<Color> spriteColors = new List<Color>()
    {
        Color.green,
        Color.red,
    };
    private int colorIndex = 0;
    private float startTime;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        bool movingLeft =
            GetComponent<OW_PlayerMechanics>().moveDirection.x < 0;
        bool movingRight =
            GetComponent<OW_PlayerMechanics>().moveDirection.x > 0;
        bool movingUp =
            GetComponent<OW_PlayerMechanics>().moveDirection.y > 0;
        bool movingDown =
            GetComponent<OW_PlayerMechanics>().moveDirection.y < 0;
        bool moving = movingLeft || movingRight || movingUp || movingDown;

        AnimateSteps(moving);
        ChangePlayerDirection(movingLeft, movingRight, movingUp, movingDown);
    }

    /*
     * Animate Steps ( bool ) 
     * This method accepts a boolean parameter that is true if the gameObject
     * is moving (OW_PlayerMechanicsfield moveDirection == nonZero).
     * 
     * If the gameObject is moving, the method will alternate the color of the 
     * sprite every one second while moving. 
     * 
     * Otherwise, the color of the sprite is black. 
     * 
     * This simulates using sprites for walking and being stationary.
     */
    void AnimateSteps(bool moving)
    {
        if (moving)
        {
            // Actively walking
            float elapsedTime = Time.time - startTime;
            if (elapsedTime >= 0.3f)
            {
                // Reset start time
                startTime = Time.time;

                // Flip colorIndex
                colorIndex = (colorIndex == 0) ? 1 : 0;
            }

            spriteRenderer.color = spriteColors[colorIndex];
        }
        else
        {
            // Not walking
            spriteRenderer.color = Color.black;
        }
    }

    /*
     * ChangePlayerDirection ( bool , bool , bool, bool) 
     * This method accepts four boolean parameters. Only one of them will be 
     * true at a time. Whichever one it is will be used to establish a target 
     * vector in the appropriate direction. The gameObject's orientation will
     * be rotated to the target orientaiton
     * 
     * For instance, if moving north the code will set the target to Vector2.up
     * and find the delta angle between the current Euler angles and the up
     * vector. The body is rotated through the delta
     * 
     */
    void ChangePlayerDirection(bool movingLeft, bool movingRight,
        bool movingUp, bool movingDown)
    {
        Vector2 currentDirection = transform.up;
        Vector2 targetDirection = currentDirection;
        if (movingLeft)
        {
            // Face left
            targetDirection = Vector2.left;
        }
        else if (movingRight)
        {
            // Face right
            targetDirection = Vector2.right;
        }
        else if (movingUp)
        {
            // Face up
            targetDirection = Vector2.up;
        }
        else if (movingDown)
        {
            // Face down
            targetDirection = Vector2.down;
        }

        float angle =
            Vector2.SignedAngle(currentDirection, targetDirection);
        transform.Rotate(0f, 0f, angle);
    }
}
