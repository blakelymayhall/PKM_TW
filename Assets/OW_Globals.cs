using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementDirections
{
    Up,
    Right,
    Down,
    Left,
    Static
}

public class OW_Globals : MonoBehaviour
{

    /* MovementDirections (Vector3) 
     * 
     * This method accepts a direction the sprite is moving and returns
     * the cardinal direction type
     * 
     */
    public static MovementDirections GetDirection(Vector3 direction)
    {
        if (direction.x < 0)
        {
            return MovementDirections.Left;
        }
        else if (direction.x > 0)
        {
            return MovementDirections.Right;
        }
        else if (direction.y > 0)
        {
            return MovementDirections.Up;
        }
        else if (direction.y < 0)
        {
            return MovementDirections.Down;
        }
        else
        {
            return MovementDirections.Static;
        }
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
        MovementDirections moveDirection)
    {
        Vector2 currentDirection = gameObject.transform.up;
        Vector2 targetDirection = currentDirection;
        switch (moveDirection)
        {
            case MovementDirections.Left:
            {
                targetDirection = Vector2.left;
                break;
            }
            case MovementDirections.Right:
            {
                targetDirection = Vector2.right;
                break;
            }
            case MovementDirections.Up:
            {
                targetDirection = Vector2.up;
                break;
            }
            case MovementDirections.Down:
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
