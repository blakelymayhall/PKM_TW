using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : OW_Animator
{
    /* PUBLIC VARS */
    //*************************************************************************

    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private OW_Player player;
    //*************************************************************************

    protected override void Start()
    {
        base.Start();
        walkTime = 0.25f;
        runTime = 0.15f;
        
        player = GetComponent<OW_Player>();
    }

    protected override void Update()
    {
        timer += Time.deltaTime;

        if (player.playerMode != OW_PlayerModes.MOVING)
        {
            timer = 0f;
            spriteIndex = stillSprite;
            spriteRenderer.sprite = directionSprites[spriteIndex];
            return;
        }

        if (timer > (mechanics.isSprinting ? runTime : walkTime))
        {
            timer = 0f;
            spriteIndex = (spriteIndex + 1) % directionSprites.Count;
            spriteRenderer.sprite = directionSprites[spriteIndex];
        }
    }

    public void ShowWalkingSprite()
    {
        spriteIndex = 1;
        spriteRenderer.sprite = directionSprites[spriteIndex];
    }
}