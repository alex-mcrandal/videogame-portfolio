/*
 * Authentication provides functionality for setting up Unity Services and storing 
 * important data. It does not reside on a game object so it is always accessible.
 */

using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;

#if UNITY_EDITOR
using ParrelSync;
#endif

//====================================================================================
//File:             Authentication.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-19-2022
//Project:          Project Monster
//====================================================================================

namespace Client.Info
{
    public class Authentication
    {
        #region Public Variables
        /// <summary>
        /// The player's id after signing in with Unity Services
        /// </summary>
        public static string playerId { get; private set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Signs the player into Unity Services anonymously
        /// </summary>
        /// <returns>A void Task from the asyncronous function</returns>
        public static async Task LoginAsync()
        {
            if (UnityServices.State == ServicesInitializationState.Uninitialized)
            {
                InitializationOptions options = new InitializationOptions();

#if UNITY_EDITOR
                //Make sure ParrelSync is installed in the editor
                if (ClonesManager.IsClone())
                {
                    options.SetProfile(ClonesManager.GetArgument());
                }
                else
                {
                    options.SetProfile("Primary");
                }
#endif

                await UnityServices.InitializeAsync(options);
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                playerId = AuthenticationService.Instance.PlayerId;
            }
        }
        #endregion
    }
}