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

    /*
     * RotateSprite (GameObject gameObject, MovementDirections moveDirection)
     * 
     * This method checks the accepts a gameObject and a direction to rotate 
     * the object to face
     * 
     * Whichever direction it is will be used to establish a target 
     * vector in the appropriate direction. The gameObject's orientation will
     * be rotated to the target orientaiton
     * 
     * For instance, if moving north the code will set the target to Vector2.up
     * and find the delta angle between the current Euler angles and the up
     * vector. The body is rotated through the delta
     * 
     */
    public static void RotateSprite(GameObject gameObject,
        MovementDirection moveDirection)
    {
        Vector2 currentDirection = gameObject.transform.up;
        Vector2 targetDirection = currentDirection;
        switch (moveDirection)
        {
            case MovementDirection.Left:
            {
                targetDirection = Vector2.left;
                break;
            }
            case MovementDirection.Right:
            {
                targetDirection = Vector2.right;
                break;
            }
            case MovementDirection.Up:
            {
                targetDirection = Vector2.up;
                break;
            }
            case MovementDirection.Down:
            {
                targetDirection = Vector2.down;
                break;
            }
        }

        float angle =
            Vector2.SignedAngle(currentDirection, targetDirection);
        gameObject.transform.Rotate(0f, 0f, angle);
    }
}

