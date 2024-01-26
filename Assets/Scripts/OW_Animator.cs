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
    protected float walkTime = 0.2f;
    protected float runTime = 0.15f;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************

    //*************************************************************************

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mechanics = GetComponent<OW_MovingObject>();
        
        UpdateDirectionSprites(Vector2.zero);
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
            timer = 0f;
            spriteIndex = (spriteIndex + 1) % directionSprites.Count;
            spriteRenderer.sprite = directionSprites[spriteIndex];
        }
    }

    public void UpdateDirectionSprites(Vector2 facingDirection)
    {
        Dictionary<Vector2, int> directionToSpriteIdx = new()
        {
            { Vector2.left, leftSpriteStartIdx },
            { Vector2.right, rightSpriteStartIdx },
            { Vector2.down, downSpriteStartIdx },
            { Vector2.up, upSpriteStartIdx },
        };
        directionSprites = directionToSpriteIdx.TryGetValue(facingDirection, out int spriteIdx)
            ? sprites.GetRange(spriteIdx,noSprites)
            : sprites.GetRange(upSpriteStartIdx,noSprites);
        spriteRenderer.sprite = directionSprites[stillSprite];
    }
}