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
        if (!isMoving)
        {
            StartCoroutine(SmoothMovement(target));
            return true;
        }

        return false;
    }

    // Co-routine for moving units from one space to next, takes a 
    // parameter end to specify where to move to.
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

    // AttemptMove takes a generic parameter T to specify the type of component 
    // we expect our unit to interact with if blocked (Player for Enemies, Wall for Player).
    // protected virtual void AttemptMove<T>(int xDir, int yDir)
    //     where T : Component
    // {
    //     //Hit will store whatever our linecast hits when Move is called.
    //     RaycastHit2D hit;

    //     //Set canMove to true if Move was successful, false if failed.
    //     bool canMove = Move(xDir, yDir, out hit);

    //     //Check if nothing was hit by linecast
    //     if (hit.transform == null)
    //         //If nothing was hit, return and don't execute further code.
    //         return;

    //     //Get a component reference to the component of type T attached to the object that was hit
    //     T hitComponent = hit.transform.GetComponent<T>();

    //     //If canMove is false and hitComponent is not equal to null, meaning MovingObject is blocked and has hit something it can interact with.
    //     if (!canMove && hitComponent != null)

    //         //Call the OnCantMove function and pass it hitComponent as a parameter.
    //         OnCantMove(hitComponent);
    // }

    //OnCantMove will be overriden by functions in the inheriting classes.
    // protected abstract void OnCantMove<T>(T component)
    //     where T : Component;
}
