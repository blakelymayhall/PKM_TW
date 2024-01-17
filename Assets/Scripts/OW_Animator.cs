using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OW_Animator : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<Sprite> sprites = new();
    //*************************************************************************

    /* PROTECTED VARS */
    //*************************************************************************
    protected OW_MovingObject mechanics;
    protected SpriteRenderer spriteRenderer;
    
    protected int noSprites = 4;
    protected int leftSpriteStartIdx = 8;
    protected int rightSpriteStartIdx = 12;
    protected int downSpriteStartIdx = 0; 
    protected int upSpriteStartIdx = 4; 
    protected int stillSprite = 0;
    protected List<Sprite> directionSprites = new();
    protected int spriteIndex = 0;

    protected float timer = 0f;
    protected float walkTime = 0.16f;
    protected float runTime = 0.12f;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************

    //*************************************************************************

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mechanics = GetComponent<OW_MovingObject>();
        
        UpdateDirectionSprites(OW_Globals.GetDirection(Vector3.zero));
    }

    protected virtual void Update()
    {
        timer += Time.deltaTime;

        if (mechanics.noInput && !mechanics.isMoving)
        {
            timer = 0f;
            spriteIndex = stillSprite;
            spriteRenderer.sprite = directionSprites[spriteIndex];
            return;
        }

        if (timer > (mechanics.isSprinting ? runTime : walkTime))
        {
            spriteIndex = (spriteIndex + 1) % directionSprites.Count;
            spriteRenderer.sprite = directionSprites[spriteIndex];
            timer = 0f;
        }
    }

    public void UpdateDirectionSprites(MovementDirection facingDirection)
    {
        // Get sprites for direciton we are moving
        directionSprites = facingDirection switch
        {
            MovementDirection.Left => 
                sprites.GetRange(leftSpriteStartIdx, noSprites),
            MovementDirection.Right =>
                sprites.GetRange(rightSpriteStartIdx, noSprites),
            MovementDirection.Down => 
                sprites.GetRange(downSpriteStartIdx, noSprites),
            _ => sprites.GetRange(upSpriteStartIdx, noSprites)
        };
        spriteRenderer.sprite = directionSprites[stillSprite];
    }
}