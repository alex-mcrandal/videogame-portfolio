/*
 * Ball is responsible for storing data and providing functionality 
 * to a ball object.
 */

using UnityEngine;

/*
 *  File:           BallData.cs
 *  Author:         Alex McRandal
 *  Email:          amcranda@heidelberg.edu
 *  Project:        GDM IV, Pong
 */

public class BallData
{

    //--------Instance Attributes--------

    /// <summary>
    /// The mulitplier for the magnitude of the ball speed
    /// </summary>
    private float ballSpeed;

    /// <summary>
    /// Multiplier for the ball's size
    /// </summary>
    private float ballSizeScalar;

    /// <summary>
    /// Var to store the ball's movement in the current frame
    /// </summary>
    private Vector2 movement;


    //--------Constructors--------


    /// <summary>
    /// Create an instance of BallData object
    /// </summary>
    /// <param name="speed">Scalar for the ball's movement speed (Default = 1f)</param>
    /// <param name="size">Scalar for the size of the ball (Default = 1f)</param>
    public BallData(float _speed = 1f, float _size = 1f)
    {
        ballSpeed = _speed;
        ballSizeScalar = _size;
        movement = Vector2.zero;
    }


    //--------Instance Methods--------


    /// <summary>
    /// Get the speed scalar of the ball
    /// </summary>
    /// <returns>A scalar (float)</returns>
    public float GetSpeed()
    {
        return ballSpeed;
    }

    /// <summary>
    /// Get the size scalar of the ball
    /// </summary>
    /// <returns>A scalar (float)</returns>
    public float GetSize()
    {
        return ballSizeScalar;
    }

    /// <summary>
    /// Get the normalized movement vector of the ball
    /// </summary>
    /// <returns>A Vector2</returns>
    public Vector2 GetMovement()
    {
        return movement;
    }

    /// <summary>
    /// Set the speed scalar of the ball
    /// </summary>
    /// <param name="_speed">A scalar (float)</param>
    public void SetSpeed(float _speed)
    {
        ballSpeed = _speed;
    }

    /// <summary>
    /// Set the size scalar of the ball
    /// </summary>
    /// <param name="_size">A scalar (float)</param>
    public void SetSize(float _size)
    {
        ballSizeScalar = _size;
    }

    /// <summary>
    /// Set the movement direction vector of the ball
    /// </summary>
    /// <param name="_move">A Vector2 that will be normalized</param>
    public void SetMovement(Vector2 _move)
    {
        movement = _move.normalized;
    }

    /// <summary>
    /// String representation of BallData object
    /// </summary>
    /// <returns>A string with all private attributes</returns>
    public override string ToString()
    {
        return $"BallData <Speed={ballSpeed}, Size={ballSizeScalar}, Movement={movement}>";
    }
}