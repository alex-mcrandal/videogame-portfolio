/*
 * AuthenticationManager provides the function that initiates the client's connection 
 * to the Unity Relay and Lobby services.
 */

using Client.Info;
using UnityEngine;
using UnityEngine.SceneManagement;

//====================================================================================
//File:             AuthenticationManager.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-19-2022
//Project:          Project Monster
//====================================================================================

namespace Managers
{
    public class AuthenticationManager : MonoBehaviour
    {
        #region Public Methods
        /// <summary>
        /// Connect the player to Relay services and send them to the lobby scene
        /// </summary>
        public async void LoginAnonymously()
        {
            //Show loading screen
            LoadManager.singleton.ShowLoadScreen();

            //log user in to Unity Services
            await Authentication.LoginAsync();

            //load lobby scene
            SceneManager.LoadSceneAsync("Lobby");
        }
        #endregion
    }
}