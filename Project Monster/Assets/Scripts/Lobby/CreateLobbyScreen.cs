/*
 * CrateLobbyScreen uses the UI to allow a player to create a lobby and set its data.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//====================================================================================
//File:             CreateLobbyScreen.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-21-2022
//Project:          Project Monster
//====================================================================================

namespace LobbyHelpers
{
    public class CreateLobbyScreen : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Function(s) called when the player creates a lobby
        /// </summary>
        public static event Action<LobbyData> LobbyCreated;
        #endregion

        #region Private Variables
        [Tooltip("Field the player enters the name of the lobby")]
        [SerializeField] private TMP_InputField nameInput;

        [Tooltip("Selection dropdown to choose difficulty")]
        [SerializeField] private TMP_Dropdown difficultyDropdown;
        #endregion

        #region Unity Behaviours
        private void Start()
        {
            //Create dropdown menu options
            void SetOptions(TMP_Dropdown _dropdown, IEnumerable<string> _values)
            {
                _dropdown.options = _values.Select(type => new TMP_Dropdown.OptionData { text = type }).ToList();
            }

            SetOptions(difficultyDropdown, Constants.difficulties);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// The "create" button was clicked on the create lobby screen
        /// </summary>
        public void OnCreateClicked()
        {
            LobbyData lobbyData = new LobbyData
            {
                name = nameInput.text,
                maxPlayers = Constants.maxPlayers,
                difficulty = difficultyDropdown.value
            };

            LobbyCreated?.Invoke(lobbyData);
        }
        #endregion
    }

    public struct LobbyData
    {
        public string name;
        public int maxPlayers;
        public int difficulty;
    }
}