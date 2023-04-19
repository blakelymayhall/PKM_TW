using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<Sprite> sprites = new List<Sprite>();
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private int spriteIndex = 0;
    private float startTime;
    public float walkTime = 0.4f;
    private float runTime = 0.23f;
    private MovementDirections facingDirection =
        MovementDirections.NaN;
    private List<Sprite> directionSprites = new List<Sprite>();
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        directionSprites = sprites.GetRange(0, 3);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        facingDirection = GetComponent<OW_PlayerMechanics>().facingDirection;

        AnimatePlayer();
    }

    /*
     * Animate Player ( ) 
     * 
     */
    void AnimatePlayer()
    { 
        if (facingDirection == MovementDirections.Left)
        {
            directionSprites = sprites.GetRange(8, 4);
        }
        else if(facingDirection == MovementDirections.Right)
        {
            directionSprites = sprites.GetRange(12, 4);
        }
        else if (facingDirection == MovementDirections.Down)
        {
            directionSprites = sprites.GetRange(0, 4);
        }
        else if (facingDirection == MovementDirections.Up)
        {
            directionSprites = sprites.GetRange(4, 4);
        }

        if (GetComponent<OW_PlayerMechanics>().isMoving == false)
        {
            GetComponent<SpriteRenderer>().sprite =
                directionSprites[0];
            spriteIndex = 0;
            return;
        }

        // Actively moving
        float elapsedTime = Time.time - startTime;
        if (elapsedTime >=
            (GetComponent<OW_PlayerMechanics>().isSprinting ?
            runTime : walkTime))
        {
            // Reset start time
            startTime = Time.time;

            // Flip colorIndex
            spriteIndex++;
            if(spriteIndex >= 4)
            {
                spriteIndex = 0;
            }
            GetComponent<SpriteRenderer>().sprite =
                directionSprites[spriteIndex]; 
        }
    }
}
