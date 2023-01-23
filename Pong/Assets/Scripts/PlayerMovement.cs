/*
 *  Responsible for local player movement
 */

using Unity.Netcode;
using UnityEngine;

/*
 *  File:           PlayerMovement.cs
 *  Author:         Alex McRandal
 *  Email:          amcranda@heidelberg.edu
 *  Project:        GDM IV, Pong
 */

public class PlayerMovement : NetworkBehaviour
{
    [Tooltip("Speed at which the player moves")]
    public float speed = 5f;

    /// <summary>
    /// Reference to the player's input map
    /// </summary>
    private Controls playerControls;

    /// <summary>
    /// Stores the player's move direction
    /// </summary>
    private float move = 0f;

    private void Awake()
    {
        playerControls = new Controls();

        playerControls.PlayerMovement.Move.performed += ctx =>
        {
            move = ctx.ReadValue<float>();
        };

        playerControls.PlayerMovement.Move.canceled += ctx =>
        {
            move = ctx.ReadValue<float>();
        };
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            this.enabled = false;
        }
    }

    private void Update()
    {
        transform.position += move * speed * Time.deltaTime * transform.up;
        transform.position = new Vector3(transform.position.x, 
            Mathf.Clamp(transform.position.y, -3f, 3f), 
            transform.position.z);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
}
