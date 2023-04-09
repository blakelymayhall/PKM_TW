using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMoveDirection
{
    Up,
    Right,
    Down,
    Left,
    Static
}

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
    private const float walkTime = 0.6f;
    private const float runTime = 0.3f;
    private PlayerMoveDirection moveDirection =
        PlayerMoveDirection.Static;
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
        if(GetComponent<OW_PlayerMechanics>().moveDirection.x < 0)
        {
            moveDirection = PlayerMoveDirection.Left;
        }
        else if(GetComponent<OW_PlayerMechanics>().moveDirection.x > 0)
        {
            moveDirection = PlayerMoveDirection.Right;
        }
        else if (GetComponent<OW_PlayerMechanics>().moveDirection.y > 0)
        {
            moveDirection = PlayerMoveDirection.Up;
        }
        else if (GetComponent<OW_PlayerMechanics>().moveDirection.y < 0)
        {
            moveDirection = PlayerMoveDirection.Down;
        }
        else 
        {
            moveDirection = PlayerMoveDirection.Static;
        }

        AnimatePlayer();
    }

    /*
     * Animate Steps ( ) 
     * 
     * This method checks the moveDirection for Static or moving
     * 
     * If the gameObject is moving, the method will alternate the color of the 
     * sprite every one second while moving. 
     * 
     * Otherwise, the color of the sprite is black. 
     * 
     * This simulates using sprites for walking and being stationary.
     */
    void AnimatePlayer()
    {
        if (moveDirection != PlayerMoveDirection.Static)
        {
            // Actively moving
            float elapsedTime = Time.time - startTime;
            if (elapsedTime >=
                (GetComponent<OW_PlayerMechanics>().isSprinting ?
                runTime : walkTime))
            {
                // Reset start time
                startTime = 0;

                // Flip colorIndex
                colorIndex = (colorIndex == 0) ? 1 : 0;
            }

            spriteRenderer.color = spriteColors[colorIndex];
            ChangePlayerDirection();
        }
        else
        {
            // Not walking
            spriteRenderer.color = Color.black;
        }
    }

    /*
     * ChangePlayerDirection () 
     * 
     * This method checks the moveDirection for direction the player is moving
     * 
     * Whichever direction it is will be used to establish a target 
     * vector in the appropriate direction. The gameObject's orientation will
     * be rotated to the target orientaiton
     * 
     * For instance, if moving north the code will set the target to Vector2.up
     * and find the delta angle between the current Euler angles and the up
     * vector. The body is rotated through the delta
     * 
     */
    void ChangePlayerDirection()
    {
        Vector2 currentDirection = transform.up;
        Vector2 targetDirection = currentDirection;
        if (moveDirection == PlayerMoveDirection.Left)
        {
            // Face left
            targetDirection = Vector2.left;
        }
        else if (moveDirection == PlayerMoveDirection.Right)
        {
            // Face right
            targetDirection = Vector2.right;
        }
        else if (moveDirection == PlayerMoveDirection.Up)
        {
            // Face up
            targetDirection = Vector2.up;
        }
        else if (moveDirection == PlayerMoveDirection.Down)
        {
            // Face down
            targetDirection = Vector2.down;
        }

        float angle =
            Vector2.SignedAngle(currentDirection, targetDirection);
        transform.Rotate(0f, 0f, angle);
    }
}
