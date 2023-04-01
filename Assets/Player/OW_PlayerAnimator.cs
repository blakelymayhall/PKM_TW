using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : MonoBehaviour
{
    /* PRIVATE VARS */
    //*************************************************************************
    private SpriteRenderer spriteRenderer;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatePlayerWalk();
    }

    // Flips sprite on vert. axis when walking
    void AnimatePlayerWalk()
    {
        // Animate the movement when walking
        bool movingLeft =
            GetComponent<OW_PlayerMechanics>().moveDirection.x < 0;
        bool movingRight =
            GetComponent<OW_PlayerMechanics>().moveDirection.x > 0;

        if (movingLeft)
        {
            // Face Left
            spriteRenderer.flipX = true;
        }
        else if (movingRight)
        {
            // Face Right
            spriteRenderer.flipX = false;
        }
    }
}
