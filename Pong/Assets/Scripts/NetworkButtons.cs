/*
 *  Creates network buttons that appear on the game's home page
 */

using Unity.Netcode;
using UnityEngine;

/*
 *  File:       NetworkButtons.cs
 *  Authors:    Unity Technologies, Alex McRandal
 *  Email:      amcranda@heidelberg.edu
 *  Project:    GDM IV, Pong
 */

public class NetworkButtons : MonoBehaviour
{
    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUILayout.Button("Host"))
            {
                Setup();
                NetworkManager.Singleton.StartHost();
            }
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        }

        GUILayout.EndArea();
    }

    private void Setup()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCallback;
    }

    private void ApprovalCallback(NetworkManager.ConnectionApprovalRequest _request, 
        NetworkManager.ConnectionApprovalResponse _response)
    {
        int clientId = (int)(_request.ClientNetworkId);
        byte[] connectionData = _request.Payload;

        bool approval = clientId < 2;
        _response.Approved = approval;
        _response.CreatePlayerObject = approval;

        if (clientId == 0)
        {
            _response.PlayerPrefabHash = 2861058791;
            _response.Position = new Vector3(-6f, 0f, 0f);
        }
        else
        {
            _response.PlayerPrefabHash = 993434515;
            _response.Position = new Vector3(6f, 0f, 0f);
        }
        _response.Rotation = Quaternion.identity;
        _response.Pending = false;
    }
}
