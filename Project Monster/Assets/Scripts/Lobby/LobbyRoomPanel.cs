/*
 * LobbyRoomPanel is the data related to a lobby and how it is displayed to the player.
 */

using System;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

//====================================================================================
//File:             LobbyRoomPanel.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-20-2022
//Project:          Project Monster
//====================================================================================

namespace LobbyHelpers
{
    public class LobbyRoomPanel : MonoBehaviour
    {
        #region Public Variables
        [Tooltip("The lobby data/properties to be displayed")]
        public Lobby lobby { get; private set; }

        [Tooltip("Unity event function(s) to be called when lobby is selected")]
        public static event Action<Lobby> lobbySelected;
        #endregion

        #region Private Variables
        [Tooltip("Displays the name of the lobby")]
        [SerializeField] private TMP_Text nameText;

        [Tooltip("Displays the difficulty of the lobby")]
        [SerializeField] private TMP_Text difficultyText;

        [Tooltip("Displays the number of players in the lobby")]
        [SerializeField] private TMP_Text playerCountText;
        #endregion

        #region Public Methods
        /// <summary>
        /// Assign the details of a lobby the first time
        /// </summary>
        /// <param name="_lobby">Lobby data and properties</param>
        public void Init(Lobby _lobby)
        {
            UpdateDetails(_lobby);
        }

        /// <summary>
        /// Display the details of a particular lobby
        /// </summary>
        /// <param name="_lobby">Lobby data and properties</param>
        public void UpdateDetails(Lobby _lobby)
        {
            lobby = _lobby;
            nameText.text = _lobby.Name;
            difficultyText.text = Constants.difficulties[GetValue(Constants.difficultyKey)];
            playerCountText.text = $"{_lobby.Players.Count}/{_lobby.MaxPlayers}";

            int GetValue(string _key)
            {
                return int.Parse(_lobby.Data[_key].Value);
            }
        }

        /// <summary>
        /// Function performed when the player clicks on a lobby prefab
        /// </summary>
        public void Clicked()
        {
            lobbySelected?.Invoke(lobby);
        }
        #endregion
    }
}