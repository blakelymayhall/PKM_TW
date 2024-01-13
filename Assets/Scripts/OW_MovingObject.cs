using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public abstract class OW_MovingObject : MonoBehaviour
{

    /* PUBLIC VARS */
    //*************************************************************************
    public float moveTime = 0.23f; // Seconds it will take object to move
    public bool noInput = true;
    public bool isMoving = false;
    public bool isSprinting = false;
    //*************************************************************************

    /* PRIVATE VARS */
    //*************************************************************************
    private new Rigidbody2D rigidbody2D;

    private Vector2 tileSize = new Vector2(1f, 1f);
    private readonly float sprintMoveTime = 0.23f;
    private readonly float walkMoveTime = 0.33f;
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

    // Move returns true if it is able to move and false if not. 
    // Move takes parameters for x direction, y direction
    protected virtual bool Move(Vector3 target, bool delayMove = false)
    {
        if (!isMoving && CanMove(target))
        {
            StartCoroutine(SmoothMovement(target));
            return true;
        }

        return false;
    }

    // Co-routine for moving units from one space to next, takes a 
    // parameter target to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 target)
    {
        isMoving = true;

        float sqrRemainingDistance = (transform.position - target).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            float inverseMoveTime = 1f /
                (isSprinting ? sprintMoveTime : walkMoveTime);
            Vector3 newPostion = Vector3.MoveTowards(
                rigidbody2D.position,
                target,
                inverseMoveTime * Time.deltaTime);
            rigidbody2D.MovePosition(newPostion);

            sqrRemainingDistance = (transform.position - target).sqrMagnitude;
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
        RaycastHit2D hit =
            Physics2D.Linecast(transform.position, target);
        GetComponent<BoxCollider2D>().enabled = true;

        if (hit.transform == null)
        {
            return true;
        }

        // GameObject hitComponent = hit.collider.gameObject;
        // Debug.Log(hitComponent.name);

        return false;
    }
}