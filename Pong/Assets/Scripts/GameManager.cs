/*
 * GameManager oversees all the functionality and logic of the game.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  File:       GameManager.cs
 *  Author:     Alex McRandal
 *  Email:      amcranda@heidelberg.edu
 *  Project:    GDM IV, Pong
 */

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the GameManager script from any outside script with ease
    /// </summary>
    public static GameManager instance;

    [Tooltip("Ball prefab to be spawned")]
    public GameObject ballPrefab;

    /// <summary>
    /// The score of the game.
    /// Key:    Player Id Num
    /// Value:  Score Value
    /// </summary>
    private Dictionary<int, int> score = new Dictionary<int, int>();

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

        score.Add(1, 0);
        score.Add(2, 0);
        UpdateScore();
    }

    /// <summary>
    /// Begin playing the game
    /// </summary>
    public void StartGame(float _ballDirection)
    {
        IEnumerator ReadyGame()
        {
            for (int i = 3; i > 0; i--)
            {
                yield return new WaitForSeconds(1f);
            }

            SpawnBall(_ballDirection);
        }

        StartCoroutine(ReadyGame());
    }

    //Quit the game and close the application
    public void QuitGame()
    {
        Application.Quit();
    }

    //Spawn a new ball prefab into the game
    private void SpawnBall(float _ballDirection)
    {
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        BallMovement ballMove = ball.GetComponent<BallMovement>();
        if (ballMove != null)
        {
            ballMove.SetBallInMotion(_ballDirection * Vector2.right);
        }
    }

    /// <summary>
    /// Reset the score to 0-0
    /// </summary>
    private void ResetScore()
    {
        score[1] = 0;
        score[2] = 0;
        UpdateScore();
    }

    /// <summary>
    /// Add a point to a player's score
    /// </summary>
    /// <param name="_playerId">The player that scored</param>
    public void AddPoint(int _playerId)
    {
        score[_playerId]++;
        UpdateScore();

        UIManager.instance.StartGame();
    }

    /// <summary>
    /// Tell the UIManager to update the score
    /// </summary>
    private void UpdateScore()
    {
        UIManager.instance.SetScore(score[1], score[2]);
    }

}
