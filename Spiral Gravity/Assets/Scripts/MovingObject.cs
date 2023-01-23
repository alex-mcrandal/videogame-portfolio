/*
 * MovingObject will make a game object go back and forth between two points in a straight line. Because
 * the two points can vary in distance, the Unity Animator is not being used
 */

using System.Collections;
using UnityEngine;

/*
 * Author:              Alex McRandal
 * Email:               mcrandalalex@gmail.com
 * Project:             Spiral Gravity
 * Last-Modified:       Aug 24, 2022
 */

public class MovingObject : MonoBehaviour
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

    /// <summary>
    /// Control variable to handle the idle state
    /// </summary>
    private bool isWaiting;

    private void Awake()
    {
        currentState = MovingStates.idle;
        initialPosition = transform.localPosition;
        speed = 1f / speedDivisor;
        isWaiting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == MovingStates.idle) //the object is paused and not moving
        {
            if (isWaiting)
                return;

            isWaiting = true;
            StartCoroutine(ChangeFromIdle());
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

    private IEnumerator ChangeFromIdle()
    {
        yield return new WaitForSeconds(2f);
        
        if (transform.localPosition == initialPosition)
        {
            currentState = MovingStates.movingTowards;
        }
        else
        {
            currentState = MovingStates.movingBack;
        }

        isWaiting = false;
    }
}
