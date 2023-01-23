/*
 * TimeDisplay takes the time value from TimeManager and displays it as [minutes]:[seconds]:[milliseconds]
 */


using TMPro;
using UnityEngine;

/*
 *  Author:             Alex McRandal
 *  Email:              mcrandalalex@gmail.com
 *  Project:            Spiral Gravity
 *  Last-Modified:      July 23, 2022
 */

public class TimeDisplay : MonoBehaviour
{
    [Tooltip("Reference to the text displaying the formatted time")]
    public TMP_Text timerText;

    private void LateUpdate()
    {
        float rawTime = TimerManager.instance.levelTime;

        string formattedTime = FormatRawTime(rawTime);

        timerText.text = formattedTime;
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
