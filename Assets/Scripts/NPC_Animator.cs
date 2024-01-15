using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Animator : OW_Animator
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<Sprite> spinSprites = new();
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    NPC_Mechanics npc_mechanics;

    private readonly int downSpriteIdx = 2;
    private readonly int leftSpriteIdx = 3;
    private readonly int rightSpriteIdx = 1;
    private readonly int upSpriteIdx = 0;
    //*************************************************************************
    protected override void Start()
    {
        base.Start();
        npc_mechanics = GetComponent<NPC_Mechanics>();
    }

    protected override void Update()
    {
        if(npc_mechanics.identity.npc_movestyle != NPC_MoveStyle.Spin)
        {
            base.Update();
        }
    }

    public void DisplaySprite(MovementDirection facingDirection)
    {
        // Get sprites for direciton we are moving
        spriteRenderer.sprite = facingDirection switch
        {
            MovementDirection.Left => spinSprites[leftSpriteIdx],
            MovementDirection.Right => spinSprites[rightSpriteIdx],
            MovementDirection.Down => spinSprites[downSpriteIdx],
            _ => spinSprites[upSpriteIdx]
        };
    }
}
