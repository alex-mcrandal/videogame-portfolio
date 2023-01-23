/*
 * MainLobbyScreen displays information on other game lobbies that are 
 * available to join. It also contains the button that allows the player 
 * to begin creating a lobby of their own.
 */

using Client.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Services.Lobbies.Models;
using UnityEngine;

//====================================================================================
//File:             MainLobbyScreen.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-20-2022
//Project:          Project Monster
//====================================================================================

namespace LobbyHelpers
{
    public class MainLobbyScreen : MonoBehaviour
    {
        #region Private Variables
        [Tooltip("Prefab that displays lobby information and allows player to join")]
        [SerializeField] private LobbyRoomPanel lobbyPanelPrefab;

        [Tooltip("Parent object that all lobbyPanelPrefabs get attached to")]
        [SerializeField] private Transform lobbyParent;

        [Tooltip("Text game object to be active when no lobbies are found")]
        [SerializeField] private GameObject noLobbiesText;

        [Tooltip("The number of seconds until the lobbies are refreshed")]
        [SerializeField] private float lobbyRefreshRate = 2f;

        /// <summary>
        /// List of all available lobbies
        /// </summary>
        private readonly List<LobbyRoomPanel> currentLobbySpawns = new();

        /// <summary>
        /// 
        /// </summary>
        private float nextRefreshTime;
        #endregion

        #region Unity Behaviours
        private void Update()
        {
            //Refresh the list of lobbies
            if (Time.time >= nextRefreshTime)
            {
                FetchLobbiesAsync();
            }
        }

        private void OnEnable()
        {
            //Clear the current list of lobbies
            foreach (Transform child in lobbyParent)
            {
                Destroy(child.gameObject);
            }
            currentLobbySpawns.Clear();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gathers lobbies from Unity Lobby and displays them as panels to the player
        /// </summary>
        private async void FetchLobbiesAsync()
        {
            try
            {
                nextRefreshTime = Time.time + lobbyRefreshRate;

                //Grab all current lobbies
                List<Lobby> allLobbies = await MatchMakingService.GatherLobbies();

                // Destroy all the current lobby panels which don't exist anymore.
                // Exclude our own homes as it'll show for a brief moment after closing the room
                IEnumerable<string> lobbyIds = allLobbies.Where(l => l.HostId != Authentication.playerId).Select(l => l.Id);
                List<LobbyRoomPanel> notActiveLobbies = currentLobbySpawns.Where(l => !lobbyIds.Contains(l.lobby.Id)).ToList();

                foreach (LobbyRoomPanel panel in notActiveLobbies)
                {
                    Destroy(panel.gameObject);
                    currentLobbySpawns.Remove(panel);
                }

                //Update or spawn the remaining active lobbies
                foreach (Lobby lobby in allLobbies)
                {
                    LobbyRoomPanel current = currentLobbySpawns.FirstOrDefault(p => p.lobby.Id == lobby.Id);
                    if (current != null)
                    {
                        current.UpdateDetails(lobby);
                    }
                    else
                    {
                        LobbyRoomPanel panel = Instantiate(lobbyPanelPrefab, lobbyParent);
                        panel.Init(lobby);
                        currentLobbySpawns.Add(panel);
                    }
                }

                noLobbiesText.SetActive(!currentLobbySpawns.Any());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
        #endregion
    }
}