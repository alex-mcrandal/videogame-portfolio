/*
 * TimerManager keeps track of the amount of time it took the player to complete their most recent run.
 */

using UnityEngine;

/*
 * Author:              Alex McRandal
 * Email:               mcrandalalex@gmail.com
 * Project:             Spiral Gravity
 * Last-Modified:       July 12, 2022
 */

public class TimerManager : MonoBehaviour
{
    /// <summary>
    /// Reference to the player's time from any script
    /// </summary>
    [HideInInspector]
    public static TimerManager instance;

    [Tooltip("The amount of time the player has spent on the level in seconds")]
    public float levelTime;

    /// <summary>
    /// Is the timer counting up?
    /// </summary>
    private bool isCounting;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Reference of TimerManager already exsits. Destorying this object!");
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);
        isCounting = false;
    }

    private void Update()
    {
        if (isCounting)
        {
            levelTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Reset any previous time recorded and tell the time manager to begin counting agian
    /// </summary>
    public void StartTimer()
    {
        levelTime = 0f;
        isCounting = true;
    }

    /// <summary>
    /// Stop the time manager from counting up
    /// </summary>
    public void StopTimer()
    {
        isCounting = false;
    }
}