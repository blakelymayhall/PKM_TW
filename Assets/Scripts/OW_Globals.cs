using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirection
{
    Up,
    Right,
    Down,
    Left,
    NaN
}

public class OW_Globals : MonoBehaviour
{

    /* MovementDirections (Vector3) 
     * 
     * This method accepts a direction vector and returns
     * the cardinal direction type
     * 
     */
    public static MovementDirection GetDirection(Vector3 direction)
    {
        if (direction.x < 0)
        {
            return MovementDirection.Left;
        }
        else if (direction.x > 0)
        {
            return MovementDirection.Right;
        }
        else if (direction.y > 0)
        {
            return MovementDirection.Up;
        }
        else if (direction.y < 0)
        {
            return MovementDirection.Down;
        }
        else
        {
            return MovementDirection.NaN;
        }
    }

    /* MovementDirections (Vector3) 
     * 
     * This method accepts a direction vector and returns
     * the cardinal direction type
     * 
    */
    public static Vector3 GetVector3FromDirection(
        MovementDirection direction)
    {
        if (direction == MovementDirection.Up)
        {
            return Vector3.up;
        }
        else if (direction == MovementDirection.Down)
        {
            return Vector3.down;
        }
        else if (direction == MovementDirection.Left)
        {
            return Vector3.left;
        }
        else if (direction == MovementDirection.Right)
        {
            return Vector3.right;
        }

        return Vector3.zero;
    }
}

