/*
 * PlayerInteraction handles the cursor state of the local player 
 * and their ability to interact with the environment (doors).
 */

using Unity.Netcode;
using UnityEngine;

//====================================================================================
//File:             PlayerInteraction.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    1-3-2022
//Project:          Project Monster
//====================================================================================

namespace Client.Player
{
    public class PlayerInteraction : NetworkBehaviour
    {
        #region Private Variables
        /// <summary>
        /// Input and control scheme for the player
        /// </summary>
        private InputMap iMap;
        #endregion

        #region Unity Behaviours
        private void Awake()
        {
            // Create controls and behavior
            iMap = new InputMap();

            iMap.Interaction.Cursor.performed += ctx =>
            {
                ToggleCursor();
            };
        }

        public override void OnNetworkSpawn()
        {
            HideCursor();
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

        #region Private Methods
        /// <summary>
        /// Toggle the visibility of the cursor
        /// </summary>
        private void ToggleCursor()
        {
            Cursor.lockState = (CursorLockMode)( ( (int)Cursor.lockState + 1 ) % 2);
        }

        /// <summary>
        /// Display the player's cursor
        /// </summary>
        private void ShowCursor()
        {
            Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// Hide the player's cursor
        /// </summary>
        private void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion
    }
}