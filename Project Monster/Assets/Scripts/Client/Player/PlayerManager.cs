/*
 * PlayerManager holds data relevant to the player. Some in-game data such as health, ammo, stamina, etc. 
 * and data relevant to game management and networking. Generic functionality and setup is also performed 
 * within this class
 */

using Managers;
using Unity.Netcode;
using UnityEngine;

//====================================================================================
//File:             GameManager.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-28-2022
//Project:          Project Monster
//====================================================================================

namespace Client.Player
{
    public class PlayerManager : NetworkBehaviour
    {
        #region Private Variables
        [Tooltip("Where the camera is postioned relative to the base of the player")]
        [SerializeField] private Vector3 cameraOffset;

        /// <summary>
        /// Cache of the main camera object in the game scene
        /// </summary>
        private Camera mainCamera;
        #endregion

        #region Unity Behaviours
        public override void OnNetworkSpawn()
        {
            GameManager.singleton.AddPlayer(NetworkManager.Singleton.LocalClientId, this);

            if (!IsOwner)
            {
                return;
            }

            // Attach the main camera if the player owns this instance of a player prefab
            mainCamera = Camera.main;
            mainCamera.transform.SetParent(transform);
            mainCamera.transform.position = cameraOffset;
            GetComponent<PlayerMovement>().SetCamera(mainCamera);
        }
        #endregion
    }
}