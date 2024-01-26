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
        walkTime = 0.25f;
        runTime = 0.15f;

        npc_mechanics = GetComponent<NPC_Mechanics>();
        
        UpdateDirectionSprites(npc_mechanics.facingDirection);
    }

    protected override void Update()
    {
        if (npc_mechanics.identity.npc_movestyle != NPC_MoveStyle.Spin)
        {
            base.Update();
        }
    }
}
