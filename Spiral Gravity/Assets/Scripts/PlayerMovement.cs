/*
 * This script is used to take input from the player and determine the necessary actions
 * that neet to be taken based of the player's input.
 * 
 * PlayerMovement uses Unity's Input System API over the traditional Unity.Input API
 */

using UnityEngine;

/*
 * Author:          Alex McRandal
 * Contact:         Email - mcrandalalex@gmail.com
 * Project:         Spiral Gravity
 * Last-Modified:   Sept 4, 2022
 */

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Reference to the player object's game object.")]
    public Rigidbody2D rb;
    [Tooltip("Reference to the GravityManager script.")]
    public GravityManager gManager;
    [Tooltip("Reference to the CameraManager script.")]
    public CameraManager cManager;
    [Tooltip("Reference to the LevelManager script")]
    public LevelManager lManager;

    //Reference to the script generated from Unity's Action Map
    private PlayerControls controls;

    /// <summary>
    /// Reference to the player's rigidbody angular velocity in the previous frame
    /// </summary>
    private Vector2 previousPosition = Vector2.zero;

    private void Awake()
    {
        //Lock the cursor at the beginning of the game
        Cursor.lockState = CursorLockMode.Locked;

        //Initialize the controls and assign the appropiate task/method to each binding
        controls = new PlayerControls();

        controls.PlayerActions.RotateClockwise.performed += ctx => RotateClockwise();
        controls.PlayerActions.RotateCounterClockwise.performed += ctx => RotateCounterClockwise();
        controls.PlayerActions.CursorLock.performed += ctx => Cursor.lockState = (CursorLockMode)( ((int)(Cursor.lockState) + 1) % 2 );
        controls.PlayerActions.OpenMenu.performed += ctx =>
        {
            UIManager.instance.ShowPanel();
            Cursor.lockState = CursorLockMode.None;
        };
        controls.PlayerActions.OpenMenu.canceled += ctx =>
        {
            Cursor.lockState = CursorLockMode.Locked;
            UIManager.instance.HidePanels();
        };
    }

    /// <summary>
    /// Rotate the gravity and camera clockwise.
    /// The player is prevented from rotating, if the player game object is still falling
    /// </summary>
    private void RotateClockwise()
    {
        if (!rb.position.Equals(previousPosition))
        {
            return;
        }
        gManager.RotateClockwise();
        cManager.Rotate(true);
    }

    /// <summary>
    /// Rotate the gravity and camera clockwise
    /// The player is prevented from rotating, if the player game object is still falling
    /// </summary>
    private void RotateCounterClockwise()
    {
        if (!rb.position.Equals(previousPosition))
        {
            return;
        }
        gManager.RotateCounterClockwise();
        cManager.Rotate(false);
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    /// <summary>
    /// This thread is used to manage when the player hits a wall and comes to
    /// a stop to play a hit sfx
    /// </summary>
    private void FixedUpdate()
    {
        previousPosition = rb.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlaySFX(0);

        if (collision.gameObject.CompareTag("DangerBlock"))
        {
            lManager.ResetLevel();
        }
    }
}
