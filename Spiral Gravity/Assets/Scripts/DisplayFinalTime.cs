/*
 * DisplayFinalTime takes the time value from TimeManager and displays it as 
 * [minutes]:[seconds]:[milliseconds] on the final screen
 */


using TMPro;
using UnityEngine;

/*
 *  Author:             Alex McRandal
 *  Email:              mcrandalalex@gmail.com
 *  Project:            Spiral Gravity
 *  Last-Modified:      July 23, 2022
 */

public class DisplayFinalTime : MonoBehaviour
{
    [Tooltip("Reference to the text that displays the final time")]
    public TMP_Text finalTime;

    private void Awake()
    {
        finalTime.text = FormatRawTime(TimerManager.instance.levelTime);
    }

    /// <summary>
    /// Take a time stored in seconds and format it to a string that shows min:sec:millisec 
    /// with these place values: 00:00:000
    /// </summary>
    /// <param name="_rawTime">The time that has elapsed in milliseconds</param>
    /// <returns>A string showing min:sec:millisec as 00:00:000</returns>
    private string FormatRawTime(float _rawTime)
    {
        int minutes = (int)(_rawTime) / 60;
        int seconds = (int)(_rawTime) % 60;
        int milliseconds = (int)(_rawTime * 1000) % 1000;

        string formattedTime = "";

        formattedTime += (minutes / 10) + "";
        formattedTime += (minutes % 10) + ":";

        formattedTime += (seconds / 10) + "";
        formattedTime += (seconds % 10) + ":";

        formattedTime += (milliseconds / 100) + "";
        formattedTime += (milliseconds / 10 % 10) + "";
        formattedTime += (milliseconds % 10) + "";

        return formattedTime;
    }
}
