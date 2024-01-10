using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OW_PlayerAnimator : MonoBehaviour
{
    /* PUBLIC VARS */
    //*************************************************************************
    public List<Sprite> sprites = new();
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private OW_PlayerMechanics playerMechanics;
    private SpriteRenderer spriteRenderer;

    private int spriteIndex = 0;
    private readonly float walkTime = 0.2f;
    private readonly float runTime = 0.1f;
    private List<Sprite> directionSprites = new();
    private float timer = 0f;
    //*************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMechanics = GetComponent<OW_PlayerMechanics>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        UpdateDirectionSprites();
        
        if (playerMechanics.noInput && !playerMechanics.isMoving)
        {
            timer = 0f;
            spriteRenderer.sprite = directionSprites[0];
            spriteIndex = 0;
            return;
        }

        if (timer > (playerMechanics.isSprinting ? runTime : walkTime))
        {
            spriteIndex = (spriteIndex + 1) % directionSprites.Count;
            spriteRenderer.sprite = directionSprites[spriteIndex];
            timer = 0f;
        }
    }

    void UpdateDirectionSprites()
    {
        // Get sprites for direciton we are moving
        directionSprites = playerMechanics.facingDirection switch
        {
            MovementDirection.Left => sprites.GetRange(8, 4),
            MovementDirection.Right => sprites.GetRange(12, 4),
            MovementDirection.Down => sprites.GetRange(0, 4),
            _ => sprites.GetRange(4, 4),
        };
    }
}