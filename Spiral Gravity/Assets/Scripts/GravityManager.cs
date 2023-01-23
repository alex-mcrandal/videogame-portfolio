/*
 * GravityManager is tasked with altering the gravity of the Unity's Physics2D settings based
 * off the user's input.
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   May 6, 2022
 */

public class GravityManager : MonoBehaviour
{
    //A collection of the four unique 2D vectors used to alter the gravity of the game
    private Vector2[] gravityVectors = {new Vector2(0f, -9.81f),
                                      new Vector2(-9.81f, 0f),
                                      new Vector2(0f, 9.81f),
                                      new Vector2(9.81f, 0f)};

    //Index used to reference a particular vector within 'gravityVectors'
    private int gravityIndex = 0;

    private void Awake()
    {
        Physics2D.gravity = gravityVectors[gravityIndex];
    }

    /// <summary>
    /// Rotates the gravity of the world clockwise.
    /// </summary>
    public void RotateClockwise()
    {
        gravityIndex = (gravityIndex + 1) % gravityVectors.Length;
        Physics2D.gravity = gravityVectors[gravityIndex];
    }

    /// <summary>
    /// Rotates the gravity of the world counter-clockwise
    /// </summary>
    public void RotateCounterClockwise()
    {
        gravityIndex = (gravityIndex + gravityVectors.Length - 1) % gravityVectors.Length;
        Physics2D.gravity = gravityVectors[gravityIndex];
    }

    /// <summary>
    /// Return the 2D gravity to its original state
    /// </summary>
    public void ResetGravity()
    {
        gravityIndex = 0;
        Physics2D.gravity = gravityVectors[gravityIndex];
    }
}
