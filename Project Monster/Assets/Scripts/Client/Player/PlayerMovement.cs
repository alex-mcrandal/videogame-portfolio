/*
 * PlayerMovement handles movement input from the player and applies the correct logic and functionality 
 * to the player's character.
 * 
 * Each of the following systems is handles by PlayerMovement
 * 
 *      -Movement along X and Z axis
 *      -Sprint speed adjustment
 *      -Crouch speed adjustment / character height adjustment
 *      -Jumping
 *      -Camera control
 */

using Unity.Netcode;
using UnityEngine;

//====================================================================================
//File:             PlayerMovement.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-27-2022
//Project:          Project Monster
//====================================================================================

namespace Client.Player
{
    public class PlayerMovement : NetworkBehaviour
    {
        #region Private Variables
        [Tooltip("Speed modifier when the player is walking")]
        [SerializeField] private float walkSpeed;

        [Tooltip("Speed modifier when the player is sprinting")]
        [SerializeField] private float sprintSpeed;

        [Tooltip("Speed modifier when the player is crouching")]
        [SerializeField] private float crouchSpeed;

        [Tooltip("Sensitivity of the camera movement")]
        [SerializeField] private float cameraSensitivity;

        [Tooltip("How much force is applied when the player jumps")]
        [SerializeField] private float jumpForce = 200f;

        [Tooltip("Rate at which the player descends in free fall")]
        [SerializeField] private float gravityScale = -9.81f;

        [Tooltip("Distance player's feet must be from the ground to detect")]
        [SerializeField] private float groundDistance = 0.2f;

        [Tooltip("Character Controller component attached to the player")]
        [SerializeField] private CharacterController controller;

        [Tooltip("Transform of the model attached to the player (used for movement based off where the camera is looking)")]
        [SerializeField] private Transform modelTransform;

        [Tooltip("Which game layers are considered to be jumpable")]
        [SerializeField] private LayerMask groundMask;

        [Tooltip("Position near the player's feet where the ground is looked for")]
        [SerializeField] private Transform groundCheck;

        /// <summary>
        /// Current speed modifier for the player
        /// </summary>
        private float speed;

        /// <summary>
        /// Value to store the vertical rotation of the camera to be clamped and mutated
        /// </summary>
        private float cameraVerticalRotation = 0f;

        /// <summary>
        /// Input and control scheme for the player
        /// </summary>
        private InputMap iMap;

        /// <summary>
        /// Helper variable that gets context from the player and stores it for future
        /// movement
        /// </summary>
        private Vector2 rawMovement;

        /// <summary>
        /// Camera componenet attached to the player
        /// </summary>
        private Transform cameraTransform;

        /// <summary>
        /// Helper variable that receives context from the player's camera movement input
        /// </summary>
        private Vector2 rawCameraMovement;

        /// <summary>
        /// Whether or not the player is grounded
        /// </summary>
        private bool isGrounded = false;

        /// <summary>
        /// Player is trying to jump
        /// </summary>
        private bool isJumping = false;

        /// <summary>
        /// Magnitude at which the player is currently descending
        /// </summary>
        private float gravity = -2f;
        #endregion

        #region Unity Behaviours
        public override void OnNetworkSpawn()
        {
            // Only the owner of this player object can move it
            if (!IsOwner) enabled = false;
        }

        private void Awake()
        {
            speed = walkSpeed;

            #region Control Setup
            iMap = new InputMap();

            iMap.Movement.Move.performed += ctx =>
            {
                rawMovement = ctx.ReadValue<Vector2>();
            };
            iMap.Movement.Move.canceled += ctx =>
            {
                rawMovement = Vector2.zero;
            };

            iMap.Movement.Sprint.performed += ctx =>
            {
                speed = sprintSpeed;
            };
            iMap.Movement.Sprint.canceled += ctx =>
            {
                speed = walkSpeed;
            };

            iMap.Movement.Crouch.performed += ctx =>
            {
                speed = crouchSpeed;
                //TODO: Adjust camera heigth and player model
            };
            iMap.Movement.Crouch.canceled += ctx =>
            {
                speed = walkSpeed;
                //TODO: Readjust camera height and player models
            };

            iMap.Movement.Camera.performed += ctx =>
            {
                rawCameraMovement = ctx.ReadValue<Vector2>();
            };
            iMap.Movement.Camera.canceled += ctx =>
            {
                rawCameraMovement = Vector2.zero;
            };

            iMap.Movement.Jump.performed += ctx =>
            {
                isJumping = true;
            };
            iMap.Movement.Jump.canceled += ctx =>
            {
                isJumping = false;
            };
            #endregion
        }

        private void Update()
        {
            // Check where the player is standing and adjust the gravity accordingly
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                gravity = -2f;
            }
            else
            {
                gravity += gravityScale * Time.deltaTime;
            }

            // Check to see if the player is trying to jump
            if (isJumping && isGrounded)
            {
                gravity = jumpForce;
            }

            // Check to see if the player is moving backwards and trying to sprint
            // If they are, then any sprinting will be canceled
            if (speed > walkSpeed && rawMovement.y < 0f)
            {
                speed = walkSpeed;
            }

            // X and Z axis movement
            controller.Move( (speed * Time.deltaTime * rawMovement.x * modelTransform.right) 
                + (speed * Time.deltaTime * rawMovement.y * modelTransform.forward) );

            // Camera movement
            transform.Rotate(Vector3.up, cameraSensitivity * Time.deltaTime * rawCameraMovement.x);

            cameraVerticalRotation -= cameraSensitivity * Time.deltaTime * rawCameraMovement.y;
            cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -75f, 75f);
            cameraTransform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

            //Have gravity move the player vertically
            controller.Move(gravity * Time.deltaTime * transform.up);
        }

        private void OnEnable()
        {
            iMap.Enable();
        }

        private void OnDisable()
        {
            iMap.Disable();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Receive the camera component attached to the player and save a reference to its transform
        /// </summary>
        /// <param name="_attachedCamera">Camera component attached to the player</param>
        public void SetCamera(Camera _attachedCamera)
        {
            cameraTransform = _attachedCamera.transform;
        }
        #endregion
    }
}