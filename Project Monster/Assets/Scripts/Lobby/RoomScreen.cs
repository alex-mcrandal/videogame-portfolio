/*
 * RoomScreen displays information about the game lobby the player joined.
 */

using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

//====================================================================================
//File:             RoomScreen.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-22-2022
//Project:          Project Monster
//====================================================================================

namespace LobbyHelpers
{
    public class RoomScreen : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Functionality to trigger when a player wants to start the game
        /// </summary>
        public static event Action startPressed;

        /// <summary>
        /// Functionality triggered when a player leaves a lobby
        /// </summary>
        public static event Action lobbyLeft;
        #endregion

        #region Private Variable
        [Tooltip("Prefab UI that displays the player in the lobby")]
        [SerializeField] private LobbyPlayerPanel playerPanelPrefab;

        [Tooltip("Container object that displays the player panels")]
        [SerializeField] private Transform playerPanelParent;

        [Tooltip("Text to be displayed while waiting for more players")]
        [SerializeField] private TMP_Text waitingText;

        [Tooltip("Button to start the game")]
        [SerializeField] private GameObject startButton;

        [Tooltip("Button to say the local player is ready")]
        [SerializeField] private GameObject readyButton;

        /// <summary>
        /// All of the panels displaying joined players
        /// </summary>
        private readonly List<LobbyPlayerPanel> playerPanels = new();

        /// <summary>
        /// All players in the lobby are ready
        /// </summary>
        private bool allReady;

        /// <summary>
        /// The local player is game ready in the lobby
        /// </summary>
        private bool ready;
        #endregion

        #region Unity Behaviours
        private void OnEnable()
        {
            //Clear the current room data
            foreach (Transform child in playerPanelParent)
            {
                Destroy(child.gameObject);
            }
            playerPanels.Clear();

            //Update the lobby to most current data
            LobbyManager.lobbyPlayersUpdated += NetworkLobbyPlayersUpdated;
            MatchMakingService.CurrentLobbyRefreshed += OnCurrentLobbyRefreshed;
            startButton.SetActive(false);
            readyButton.SetActive(false);

            ready = false;
        }

        private void OnDisable()
        {
            //Unsubscribe to delegates
            LobbyManager.lobbyPlayersUpdated -= NetworkLobbyPlayersUpdated;
            MatchMakingService.CurrentLobbyRefreshed -= OnCurrentLobbyRefreshed;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Cue that the player has left the lobby
        /// </summary>
        public void OnLobbyLeave()
        {
            lobbyLeft?.Invoke();
        }

        /// <summary>
        /// Event function when the player clicks the ready button
        /// </summary>
        public void OnReadyClicked()
        {
            readyButton.SetActive(false);
            ready = true;
        }

        /// <summary>
        /// Player clicked the start game button
        /// </summary>
        public void OnStartClicked()
        {
            startPressed?.Invoke();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Update the player's view in the lobby depending on the status of all the players
        /// </summary>
        /// <param name="_players">Id and ready status of every player in the room</param>
        private void NetworkLobbyPlayersUpdated(Dictionary<ulong, bool> _players)
        {
            Dictionary<ulong, bool>.KeyCollection allActivePlayersIds = _players.Keys;

            //Remove all inactive panels
            List<LobbyPlayerPanel> toDestroy = playerPanels.Where(p => !allActivePlayersIds.Contains(p.playerId)).ToList();
            foreach (LobbyPlayerPanel panel in toDestroy)
            {
                playerPanels.Remove(panel);
                Destroy(panel.gameObject);
            }

            foreach (KeyValuePair<ulong, bool> player in _players)
            {
                LobbyPlayerPanel currentPanel = playerPanels.FirstOrDefault(p => p.playerId == player.Key);
                if (currentPanel != null)
                {
                    if (player.Value)
                    {
                        currentPanel.SetReady();
                    }
                }
                else
                {
                    LobbyPlayerPanel panel = Instantiate(playerPanelPrefab, playerPanelParent);
                    panel.Init(player.Key);
                    playerPanels.Add(panel);
                }
            }

            startButton.SetActive(NetworkManager.Singleton.IsHost && _players.All(p => p.Value));
            readyButton.SetActive(!ready);
        }

        /// <summary>
        /// Update the text to show how many players are in the lobby
        /// </summary>
        /// <param name="_lobby">Lobby information for the room the player is in</param>
        private void OnCurrentLobbyRefreshed(Lobby _lobby)
        {
            waitingText.text = $"Waiting on players... {_lobby.Players.Count}/{_lobby.MaxPlayers}";
        }
        #endregion
    }
}