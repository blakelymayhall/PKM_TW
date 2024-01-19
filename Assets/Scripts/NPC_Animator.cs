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
        if (npc_mechanics.identity.npc_movestyle != NPC_MoveStyle.Spin)
        {
            base.Update();
        }
    }

    public void DisplaySprite(Vector3 facingDirection)
    {
        Dictionary<Vector3, int> directionToSpriteIdx = new()
        {
            { Vector3.left, leftSpriteIdx },
            { Vector3.right, rightSpriteIdx },
            { Vector3.down, downSpriteIdx },
            { Vector3.up, upSpriteIdx },
        };
        spriteRenderer.sprite = directionToSpriteIdx.TryGetValue(facingDirection, out int spriteIdx)
            ? spinSprites[spriteIdx]
            : spinSprites[upSpriteIdx];
    }
}
