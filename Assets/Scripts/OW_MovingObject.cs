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
    public Vector2 facingDirection;
    //*************************************************************************

    /* PROTECTED VARS */
    //*************************************************************************
    protected float sprintMoveTime = 0.23f;
    protected float walkMoveTime = 0.5f;
    protected Vector2 tileSize = new(0.32f, 0.32f);
    //*************************************************************************

    protected virtual void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Returns world coordinates of the next tile in the given 
    // normalized direction
    protected Vector3 GetTargetTile(Vector2 inputDirection, Tilemap tilemap)
    {
        Vector2 nextTilePosition = (Vector2)transform.position + new Vector2(
            inputDirection.x * tileSize.x,
            inputDirection.y * tileSize.y);
        Vector3Int nextTileCellPosition = tilemap.WorldToCell(nextTilePosition);
        return (Vector2)tilemap.GetCellCenterWorld(nextTileCellPosition);
    }

    // Returns true if it is able to move and false if not. 
    protected virtual bool Move(Vector2 target)
    {
        if (CanMove(target))
        {
            StartCoroutine(SmoothMovement(target));
            return true;
        }
        else 
        {
            return false;
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
            Vector2 newPostion = Vector2.MoveTowards(
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
    protected virtual bool CanMove(Vector2 target)
    {
        // Cast a ray in the direction of the move to confirm that 
        // nothing rigid is in our path. Disable this objects collider.
        GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, target);
        GetComponent<BoxCollider2D>().enabled = true;

        return hit.transform == null;
    }

    protected void SnapToGrid(Tilemap tilemap)
    {
        Vector3Int nextTileCellPosition = tilemap.WorldToCell(transform.position);
        rigidbody2D.MovePosition(tilemap.GetCellCenterWorld(nextTileCellPosition));
    }
}
