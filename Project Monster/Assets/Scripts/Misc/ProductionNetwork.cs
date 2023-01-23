/*
 * ProductionNetwork allows the game to run multiplayer in development without using Relay and 
 * Lobby services. This will help ensure the game does not go over the free limit of Unity's pricing 
 * and make anyone incur an unexpected cost.
 */

using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

//====================================================================================
//File:             ProductionNetwork.cs
//Author:           Revolution Gaming
//Email:            revolutiongaming8@gmail.com
//Last Modified:    12-23-2022
//Project:          Project Monster
//====================================================================================

namespace Miscellaneous
{
    public class ProductionNetwork : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private NetworkManager tempNetManager;
        [SerializeField] private UnityTransport tempTransporter;
        #endregion

        #region Unity Behaviours
        private void Awake()
        {
#if UNITY_EDITOR
            if (NetworkManager.Singleton == null)
            {
                tempTransporter.enabled = true;
                tempNetManager.enabled = true;
            }
#endif
        }
        #endregion
    }
}