/*
 * GatewayMovement will make a gateway object go back and forth between two points in a straight line. Because
 * the two points can vary in distance, the Unity Animator is not being used
 */

using UnityEngine;

/*
 * Author:              Alex McRandal
 * Email:               mcrandalalex@gmail.com
 * Project:             Spiral Gravity
 * Last-Modified:       Aug 24, 2022
 */

public class GatewayMovement : MonoBehaviour
{
    /// <summary>
    /// The states used for a state machine design
    /// </summary>
    private enum MovingStates
    {
        idle,               //Not moving
        movingTowards,      //Moving towards endPointPosition
        movingBack          //Moving towards initialPosition
    }

    /// <summary>
    /// Reference to the current state the machine is in
    /// </summary>
    private MovingStates currentState;

    [Tooltip("A power of 2 int >= 64 to control the speed (> = slower)")]
    public float speedDivisor = 128;

    [Tooltip("The local position this game object will start in")]
    public Vector3 initialPosition;

    [Tooltip("The local position this object will move towards (and away vice versa)")]
    public Vector3 endPointPosition;

    /// <summary>
    /// The speed at which the object moves
    /// </summary>
    private float speed;

    private void Awake()
    {
        currentState = MovingStates.idle;
        initialPosition = transform.localPosition;
        speed = 1f / speedDivisor;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == MovingStates.idle) //the object is paused and not moving
        {
            return;
        }

        Vector3 move;
        if (currentState == MovingStates.movingTowards) //The object is moving towards the end point
        {
            move = Vector3.Normalize(endPointPosition - initialPosition);
            transform.localPosition += speed * move;
            if (transform.localPosition == endPointPosition)
                currentState = MovingStates.idle;
        }
        else                                            //The object is moving towards its initial position
        {
            move = Vector3.Normalize(initialPosition - endPointPosition);
            transform.localPosition += speed * move;
            if (transform.localPosition == initialPosition)
                currentState = MovingStates.idle;
        }
    }

    /// <summary>
    /// Tell the gateway to begin moving
    /// </summary>
    public void BeginToMove()
    {
        if (transform.localPosition == initialPosition)
        {
            currentState = MovingStates.movingTowards;
        }
        else
        {
            currentState = MovingStates.movingBack;
        }
    }
}
