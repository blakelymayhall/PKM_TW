using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : MonoBehaviour
{
    /* PRIVATE VARS */
    //*************************************************************************
    private List<Color> spriteColors = new List<Color>()
    {
        Color.green,
        Color.red,
    };
    private int colorIndex = 0;
    private float startTime;
    private const float walkTime = 0.6f;
    private const float runTime = 0.3f;
    private MovementDirections moveDirection =
        MovementDirections.Static;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveDirection = OW_Globals.GetDirection(
            GetComponent<OW_PlayerMechanics>().moveDirection);

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
        if (moveDirection != MovementDirections.Static)
        {
            // Actively moving
            float elapsedTime = Time.time - startTime;
            if (elapsedTime >=
                (GetComponent<OW_PlayerMechanics>().isSprinting ?
                runTime : walkTime))
            {
                // Reset start time
                startTime = Time.time;

                // Flip colorIndex
                colorIndex = (colorIndex == 0) ? 1 : 0;
            }

            GetComponent<SpriteRenderer>().color = spriteColors[colorIndex];
        }
        else
        {
            // Not walking
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
}
