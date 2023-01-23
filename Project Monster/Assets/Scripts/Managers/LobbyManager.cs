/*
 * LobbyManager handles the UI functionality/logic for the lobby scene and
 * communication with Unity's Lobby service.
 */

using LobbyHelpers;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

//====================================================================================
//File:             LobbyManager.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-20-2022
//Project:          Project Monster
//====================================================================================

namespace Managers
{
    public class LobbyManager : NetworkBehaviour
    {
        #region Public Variables

        #region Room
        /// <summary>
        /// Event fired when the lobby the player is in, is updated
        /// </summary>
        public static event Action<Dictionary<ulong, bool>> lobbyPlayersUpdated;
        #endregion

        #endregion

        #region Private Variables
        [Tooltip("Screen that displays active game lobbies")]
        [SerializeField] private MainLobbyScreen mainLobbyScreen;
        
        [Tooltip("Panel that allows the user to create a new game lobby")]
        [SerializeField] private CreateLobbyScreen createScreen;

        [Tooltip("Panel showing the lobby the player has joined")]
        [SerializeField] private RoomScreen roomScreen;

        #region Room
        /// <summary>
        /// Player Id, ready status pair to show each player's ready status
        /// </summary>
        private readonly Dictionary<ulong, bool> playersInLobby = new();
        #endregion

        #endregion

        #region Unity Behaviours
        private void Start()
        {
            //Show and hide proper panels
            mainLobbyScreen.gameObject.SetActive(true);
            createScreen.gameObject.SetActive(false);
            roomScreen.gameObject.SetActive(false);

            //Subscribe to custom lobby events
            CreateLobbyScreen.LobbyCreated += CreateLobby;
            LobbyRoomPanel.lobbySelected += OnLobbySelected;
            RoomScreen.lobbyLeft += OnLobbyLeft;
            RoomScreen.startPressed += OnGameStart;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
                playersInLobby.Add(NetworkManager.Singleton.LocalClientId, false);
                UpdateInterface();
            }

            //Client uses this in case host destroys the lobby
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            CreateLobbyScreen.LobbyCreated -= CreateLobby;
            LobbyRoomPanel.lobbySelected -= OnLobbySelected;
            RoomScreen.lobbyLeft -= OnLobbyLeft;
            RoomScreen.startPressed -= OnGameStart;

            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisconnectCallback;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Event function for when the player clicks the ready button
        /// </summary>
        public void OnReadyClicked()
        {
            SetReadyServerRpc(NetworkManager.Singleton.LocalClientId);
        }
        #endregion

        #region Private Methods

        #region Main Lobby
        /// <summary>
        /// Player clicked on a lobby to join (FUNCTIONAL BUT NOT DONE)
        /// </summary>
        /// <param name="_lobby">The lobby requested to join</param>
        private async void OnLobbySelected(Lobby _lobby)
        {
            //TODO: Tell the player they are joing the lobby

            try
            {
                await MatchMakingService.JoinLobbyWithAllocation(_lobby.Id);

                mainLobbyScreen.gameObject.SetActive(false);
                roomScreen.gameObject.SetActive(true);

                NetworkManager.Singleton.StartClient();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                //TODO: Display error to the user
            }
        }
        #endregion

        #region Create
        /// <summary>
        /// Create a lobby with the given data (FUNCTIONAL BUT NOT DONE)
        /// </summary>
        /// <param name="_data">Predefined information about the lobby</param>
        private async void CreateLobby(LobbyData _data)
        {
            //TODO: Display a screen saying the game is creating a lobby

            try
            {
                await MatchMakingService.CreateLobbyWithAllocation(_data);

                createScreen.gameObject.SetActive(false);
                roomScreen.gameObject.SetActive(true);

                //Starting the host immediately will keep the relay server alive
                NetworkManager.Singleton.StartHost();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                //TODO: Display error to the user
            }
        }
        #endregion

        #region Room
        /// <summary>
        /// Functionality when a player connects to the server. 
        /// The local player adds themself only to their player dictionary first.
        /// </summary>
        /// <param name="_playerId">Id of the player joining</param>
        private void OnClientConnectedCallback(ulong _playerId)
        {
            if (!IsServer) return;

            //Add locally
            if (!playersInLobby.ContainsKey(_playerId))
            {
                playersInLobby.Add(_playerId, false);
            }

            PropagateToClients();

            UpdateInterface();
        }

        /// <summary>
        /// Tell all other clients in the lobby that the local player has joined
        /// </summary>
        private void PropagateToClients()
        {
            foreach (KeyValuePair<ulong, bool> player in playersInLobby)
            {
                UpdatePlayerClientRpc(player.Key, player.Value);
            }
        }

        /// <summary>
        /// Tell all clients that a player has joined or changed ready status
        /// </summary>
        /// <param name="_clientId">The id of the player</param>
        /// <param name="_isReady">The ready status of the player (True means ready)</param>
        [ClientRpc]
        private void UpdatePlayerClientRpc(ulong _clientId, bool _isReady)
        {
            if (IsServer) return;

            if (!playersInLobby.ContainsKey(_clientId))
            {
                playersInLobby.Add(_clientId, _isReady);
            }
            else
            {
                playersInLobby[_clientId] = _isReady;
                UpdateInterface();
            }
        }

        /// <summary>
        /// Functionality when a player disconnects from the lobby
        /// </summary>
        /// <param name="_playerId">The id of the player that left</param>
        private void OnClientDisconnectCallback(ulong _playerId)
        {
            if (IsServer)
            {
                //Handle locally
                if (playersInLobby.ContainsKey(_playerId))
                {
                    playersInLobby.Remove(_playerId);
                }

                //Propagate to all clients
                RemovePlayerClientRpc(_playerId);

                UpdateInterface();
            }
            else
            {
                // This happens when the host disconnects the lobby
                roomScreen.gameObject.SetActive(false);
                mainLobbyScreen.gameObject.SetActive(true);
                OnLobbyLeft();
            }
        }

        [ClientRpc]
        private void RemovePlayerClientRpc(ulong _clientId)
        {
            if (IsServer) return;

            if (playersInLobby.ContainsKey(_clientId))
            {
                playersInLobby.Remove(_clientId);
            }

            UpdateInterface();
        }

        /// <summary>
        /// Tell the server that the local player is ready. Server then tells all other players
        /// </summary>
        /// <param name="_playerId">The id of the player that is now ready</param>
        [ServerRpc(RequireOwnership = false)]
        private void SetReadyServerRpc(ulong _playerId)
        {
            playersInLobby[_playerId] = true;
            PropagateToClients();
            UpdateInterface();
        }

        /// <summary>
        /// Update the room screen interface for every client in the room
        /// </summary>
        private void UpdateInterface()
        {
            lobbyPlayersUpdated?.Invoke(playersInLobby);
        }

        /// <summary>
        /// Local player has left the lobby they were in (FUNCTIONAL BUT NOT DONE)
        /// </summary>
        private async void OnLobbyLeft()
        {
            //TODO: Tell the player they are leaving the lobby
            playersInLobby.Clear();
            NetworkManager.Singleton.Shutdown();
            await MatchMakingService.LeaveLobby();
        }

        /// <summary>
        /// Start the game and close the lobby so no new players can enter
        /// </summary>
        private async void OnGameStart()
        {
            //The game is starting
            LoadManager.singleton.ShowLoadScreen();

            await MatchMakingService.LockLobby();
            NetworkManager.Singleton.SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        #endregion

        #endregion
    }
}