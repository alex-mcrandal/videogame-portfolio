/*
 * LobbyPlayerPanel displays the usernames of players inside the current lobby.
 */

using TMPro;
using UnityEngine;

//====================================================================================
//File:             LobbyPlayerPanel.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-22-2022
//Project:          Project Monster
//====================================================================================

namespace LobbyHelpers
{
    public class LobbyPlayerPanel : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Id of the player the panel is displaying
        /// </summary>
        public ulong playerId { get; private set; }
        #endregion

        #region Private Variables
        [Tooltip("Text displaying the name of the player")]
        [SerializeField] private TMP_Text nameText;

        [Tooltip("Text showing the ready status of the player")]
        [SerializeField] private TMP_Text statusText;
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the data of the panel with the player's information
        /// </summary>
        /// <param name="_playerId"></param>
        public void Init(ulong _playerId)
        {
            playerId = _playerId;
            nameText.text = $"Player {_playerId}";      //Here is where an upgrade can be made to show username and not id number
        }

        /// <summary>
        /// Have the panel show the player is ready
        /// </summary>
        public void SetReady()
        {
            statusText.text = "Ready";
            statusText.color = Color.green;
        }
        #endregion
    }
}