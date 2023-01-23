/*
 * LoadManager displays a specified loading screen to the player.
 */

using UnityEngine;

//====================================================================================
//File:             LoadManager.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-22-2022
//Project:          Project Monster
//====================================================================================

namespace Managers
{
    public class LoadManager : MonoBehaviour
    {
        #region Public Variables
        /// <summary>
        /// Singleton reference to this script's functionality
        /// </summary>
        public static LoadManager singleton;
        #endregion

        #region Private Variables
        [Tooltip("Panel to display to the user when the game is loading")]
        [SerializeField] private GameObject loadingScreen;

        /// <summary>
        /// Helper object to ensure proper creation of a singleton
        /// </summary>
        static readonly object padlock = new object();
        #endregion

        #region Unity Behaviours
        private void Awake()
        {
            //Create local scene singleton
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
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Display loading screen to the player
        /// </summary>
        public void ShowLoadScreen()
        {
            loadingScreen.SetActive(true);
        }
        #endregion
    }
}