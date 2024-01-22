using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public abstract class OW_MovingObject : MonoBehaviour
{

    /* PUBLIC VARS */
    //*************************************************************************
    public new Rigidbody2D rigidbody2D;
    public bool noInput = true;
    public bool isMoving = false;
    public bool isSprinting = false;
    //*************************************************************************

    /* PROTECTED VARS */
    //*************************************************************************
    protected float sprintMoveTime = 0.23f;
    protected float walkMoveTime = 0.5f;
    protected Vector2 tileSize = new(0.32f, 0.32f);
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************

    //*************************************************************************

    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Returns world coordinates of the next tile in the given 
    // normalized direction
    protected Vector3 GetTargetTile(Vector3 inputDirection, Tilemap tilemap)
    {
        Vector3 nextTilePosition = transform.position + new Vector3(
            inputDirection.x * tileSize.x,
            inputDirection.y * tileSize.y,
            0f);
        Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
        return tilemap.GetCellCenterWorld(nextTileCellPosition);
    }

    // Returns true if it is able to move and false if not. 
    protected virtual void Move(Vector3 target)
    {
        if (CanMove(target))
        {
            StartCoroutine(SmoothMovement(target));
        }
    }

    // Co-routine for moving units from one space to next, takes a 
    // parameter target to specify where to move to.
    protected virtual IEnumerator SmoothMovement(Vector2 target)
    {
        isMoving = true;

        float sqrRemainingDistance = (rigidbody2D.position - target).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            float inverseMoveTime = 1f / (isSprinting ? sprintMoveTime : walkMoveTime);
            Vector3 newPostion = Vector3.MoveTowards(
                rigidbody2D.position,
                target,
                inverseMoveTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPostion);

            sqrRemainingDistance = (rigidbody2D.position - target).sqrMagnitude;
            yield return null;
        }
    
        rigidbody2D.MovePosition(target);
        isMoving = false;
    }

    // Method returns false if anything is occupying the tile being moved to
    protected virtual bool CanMove(Vector3 target)
    {
        // Cast a ray in the direction of the move to confirm that 
        // nothing rigid is in our path. Disable this objects collider.
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target);
        GetComponent<BoxCollider2D>().enabled = true;

        return hit.transform == null;
    }
}
