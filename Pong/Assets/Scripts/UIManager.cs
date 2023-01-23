/*
 * UIManager oversees all the functionality and logic of the Canvas view.
 */

using Unity.Netcode;
using UnityEngine;
using TMPro;

/*
 *  File:       UIManager.cs
 *  Author:     Alex McRandal
 *  Email:      amcranda@heidelberg.edu
 *  Project:    GDM IV, Pong
 */

public class UIManager : NetworkBehaviour
{
    /// <summary>
    /// Reference to the GameManager script from any outside script with ease
    /// </summary>
    public static UIManager instance;

    [Tooltip("Panel to show everything in the player view")]
    public GameObject playerView;

    [Tooltip("Panel to show everything in the start view")]
    public GameObject startView;

    [Tooltip("Text showing Player 1's score")]
    public TMP_Text p1Score;

    [Tooltip("Text showing Player 2's score")]
    public TMP_Text p2Score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        SetStartViewActive(true);
        SetPlayerViewActive(false);
    }

    /// <summary>
    /// Tell the GameManager to start the game
    /// </summary>
    public void StartGame()
    {
        // Send request to server
        RequestStartServerRpc();
    }

    [ServerRpc]
    private void RequestStartServerRpc()
    {
        float direction = Mathf.Pow(-1f, (float)Random.Range(0, 2));
        StartClientRpc(direction);
    }

    [ClientRpc]
    private void StartClientRpc(float _ballDirection)
    {
        SetStartViewActive(false);
        SetPlayerViewActive(true);

        GameManager.instance.StartGame(_ballDirection);
    }

    /// <summary>
    /// Tell the GameManager to quit the game
    /// </summary>
    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    /// <summary>
    /// Set the active field of the PlayerView game object
    /// </summary>
    /// <param name="_active">True to show, False to hide</param>
    public void SetPlayerViewActive(bool _active)
    {
        playerView.SetActive(_active);
    }

    /// <summary>
    /// Set the active field of the StartView game object
    /// </summary>
    /// <param name="_active">True to show, False to hide</param>
    public void SetStartViewActive(bool _active)
    {
        startView.SetActive(_active);
    }

    /// <summary>
    /// Show the current score of the game
    /// </summary>
    /// <param name="_player1Score">Player 1's score</param>
    /// <param name="_player2Score">Player 2's score</param>
    public void SetScore(int _player1Score, int _player2Score)
    {
        p1Score.text = _player1Score + "";
        p2Score.text = _player2Score + "";
    }
}
