/*
 * GameMananger stores data relevant to all instances of the lobby game and provides 
 * functionality for players.
 */

using Client.Player;
using System.Collections.Generic;
using LobbyHelpers;
using Unity.Netcode;
using UnityEngine;

//====================================================================================
//File:             GameManager.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-28-2022
//Project:          Project Monster
//====================================================================================

namespace Managers
{
    public class GameManager : NetworkBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Singleton reference to the GameManger object
        /// </summary>
        public static GameManager singleton;

        [Tooltip("Client id and player manager pairs")]
        public Dictionary<ulong, PlayerManager> players { get; private set; }
        #endregion

        #region Private Variables
        /// <summary>
        /// Object to help control the creation of the singleton
        /// </summary>
        private static readonly object padlock = new();
        #endregion

        #region Unity Behaviours
        private void Awake()
        {
            //Create singleton
            if (singleton == null)
            {
                lock (padlock)
                {
                    if (singleton == null)
                    {
                        singleton = this;
                    }
                    else
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                Destroy(this.gameObject);
            }

            players = new Dictionary<ulong, PlayerManager>();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
#pragma warning disable CS4014
            MatchMakingService.LeaveLobby();
#pragma warning restore CS4014
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.Shutdown();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a player to the dictionary of players
        /// </summary>
        /// <param name="_id">Id of the new player</param>
        /// <param name="_manager">Manager belonging to the new player</param>
        public void AddPlayer(ulong _id, PlayerManager _manager)
        {
            players.Add(_id, _manager);
        }
        #endregion
    }
}